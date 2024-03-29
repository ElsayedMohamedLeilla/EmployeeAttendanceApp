using AutoMapper;
using Dawem.Contract.BusinessLogic.Dawem.Summons;
using Dawem.Contract.BusinessValidation.Dawem.Summons;
using Dawem.Contract.Repository.Manager;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Summons;
using Dawem.Helpers;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Dawem.Employees.Employees;
using Dawem.Models.Dtos.Dawem.Summons.Sanctions;
using Dawem.Models.Generic.Exceptions;
using Dawem.Models.Response.Dawem.Summons.Sanctions;
using Dawem.Translations;
using Microsoft.EntityFrameworkCore;

namespace Dawem.BusinessLogic.Dawem.Summons
{
    public class SanctionBL : ISanctionBL
    {
        private readonly IUnitOfWork<ApplicationDBContext> unitOfWork;
        private readonly RequestInfo requestInfo;
        private readonly ISanctionBLValidation sanctionBLValidation;
        private readonly IRepositoryManager repositoryManager;
        private readonly IMapper mapper;
        public SanctionBL(IUnitOfWork<ApplicationDBContext> _unitOfWork,
            IRepositoryManager _repositoryManager,
            IMapper _mapper,
           RequestInfo _requestHeaderContext,
           ISanctionBLValidation _sanctionBLValidation)
        {
            unitOfWork = _unitOfWork;
            requestInfo = _requestHeaderContext;
            repositoryManager = _repositoryManager;
            sanctionBLValidation = _sanctionBLValidation;
            mapper = _mapper;
        }
        public async Task<int> Create(CreateSanctionModel model)
        {
            #region Business Validation

            await sanctionBLValidation.CreateValidation(model);

            #endregion

            unitOfWork.CreateTransaction();

            #region Insert Sanction

            #region Set Sanction code
            var getNextCode = await repositoryManager.SanctionRepository
                .Get(e => e.CompanyId == requestInfo.CompanyId)
                .Select(e => e.Code)
                .DefaultIfEmpty()
                .MaxAsync() + 1;
            #endregion

            var sanction = mapper.Map<Sanction>(model);
            sanction.CompanyId = requestInfo.CompanyId;
            sanction.AddUserId = requestInfo.UserId;

            sanction.Code = getNextCode;
            repositoryManager.SanctionRepository.Insert(sanction);
            await unitOfWork.SaveAsync();

            #endregion

            #region Handle Response

            await unitOfWork.CommitAsync();
            return sanction.Id;

            #endregion

        }
        public async Task<bool> Update(UpdateSanctionModel model)
        {
            #region Business Validation

            await sanctionBLValidation.UpdateValidation(model);

            #endregion

            unitOfWork.CreateTransaction();

            #region Update Sanction

            var getSanction = await repositoryManager.SanctionRepository
                .GetEntityByConditionWithTrackingAsync(sanction => !sanction.IsDeleted
                && sanction.Id == model.Id);


            if (getSanction != null)
            {
                getSanction.Name = model.Name;
                getSanction.Type = model.Type;
                getSanction.IsActive = model.IsActive;
                getSanction.ModifiedDate = DateTime.Now;
                getSanction.ModifyUserId = requestInfo.UserId;
                await unitOfWork.SaveAsync();
                #region Handle Response
                await unitOfWork.CommitAsync();
                return true;
                #endregion
            }
            #endregion

            else
                throw new BusinessValidationException(LeillaKeys.SorrySanctionNotFound);


        }
        public async Task<GetSanctionsResponse> Get(GetSanctionsCriteria criteria)
        {
            var sanctionRepository = repositoryManager.SanctionRepository;
            var query = sanctionRepository.GetAsQueryable(criteria);

            #region paging
            int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            int take = PagingHelper.Take(criteria.PageSize);
            #region sorting
            var queryOrdered = sanctionRepository.OrderBy(query, nameof(Sanction.Id), LeillaKeys.Desc);
            #endregion
            var queryPaged = criteria.GetPagingEnabled() ? queryOrdered.Skip(skip).Take(take) : queryOrdered;
            #endregion

            #region Handle Response

            var sanctionsList = await queryPaged.Select(e => new GetSanctionsResponseModel
            {
                Id = e.Id,
                Code = e.Code,
                Name = e.Name,
                TypeName = TranslationHelper.GetTranslation(e.Type.ToString() + LeillaKeys.SanctionType, requestInfo.Lang),
                IsActive = e.IsActive,
            }).ToListAsync();
            return new GetSanctionsResponse
            {
                Sanctions = sanctionsList,
                TotalCount = await query.CountAsync()
            };
            #endregion

        }
        public async Task<GetSanctionsForDropDownResponse> GetForDropDown(GetSanctionsCriteria criteria)
        {

            criteria.IsActive = true;
            var sanctionRepository = repositoryManager.SanctionRepository;
            var query = sanctionRepository.GetAsQueryable(criteria);

            #region paging

            int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            int take = PagingHelper.Take(criteria.PageSize);

            #region sorting
            var queryOrdered = sanctionRepository.OrderBy(query, nameof(Sanction.Id), LeillaKeys.Desc);
            #endregion

            var queryPaged = criteria.GetPagingEnabled() ? queryOrdered.Skip(skip).Take(take) : queryOrdered;

            #endregion

            #region Handle Response

            var sanctionsList = await queryPaged.Select(e => new GetSanctionsForDropDownResponseModel
            {
                Id = e.Id,
                Name = e.Name
            }).ToListAsync();

            return new GetSanctionsForDropDownResponse
            {
                Sanctions = sanctionsList,
                TotalCount = await query.CountAsync()
            };

            #endregion

        }
        public async Task<GetSanctionInfoResponseModel> GetInfo(int sanctionId)
        {
            var sanction = await repositoryManager.SanctionRepository.Get(e => e.Id == sanctionId && !e.IsDeleted)
                .Select(e => new GetSanctionInfoResponseModel
                {
                    Code = e.Code,
                    Name = e.Name,
                    TypeName = TranslationHelper.GetTranslation(e.Type.ToString() + LeillaKeys.SanctionType, requestInfo.Lang),
                    WarningMessage = e.WarningMessage,
                    IsActive = e.IsActive,
                }).FirstOrDefaultAsync() ?? throw new BusinessValidationException(LeillaKeys.SorrySanctionNotFound);

            return sanction;
        }
        public async Task<GetSanctionByIdResponseModel> GetById(int sanctionId)
        {
            var sanction = await repositoryManager.SanctionRepository.Get(e => e.Id == sanctionId && !e.IsDeleted)
                .Select(e => new GetSanctionByIdResponseModel
                {
                    Id = e.Id,
                    Code = e.Code,
                    Name = e.Name,
                    Type = e.Type,
                    WarningMessage = e.WarningMessage,
                    IsActive = e.IsActive,
                }).FirstOrDefaultAsync() ?? throw new BusinessValidationException(LeillaKeys.SorrySanctionNotFound);

            return sanction;

        }
        public async Task<bool> Enable(int sanctiond)
        {
            var sanction = await repositoryManager.SanctionRepository.GetEntityByConditionWithTrackingAsync(d => !d.IsDeleted && !d.IsActive && d.Id == sanctiond) ??
                throw new BusinessValidationException(LeillaKeys.SorrySanctionNotFound);
            sanction.Enable();
            await unitOfWork.SaveAsync();
            return true;
        }
        public async Task<bool> Disable(DisableModelDTO model)
        {
            var sanction = await repositoryManager.SanctionRepository.GetEntityByConditionWithTrackingAsync(d => !d.IsDeleted && d.IsActive && d.Id == model.Id) ??
                throw new BusinessValidationException(LeillaKeys.SorrySanctionNotFound);
            sanction.Disable(model.DisableReason);
            await unitOfWork.SaveAsync();
            return true;
        }
        public async Task<bool> Delete(int sanctiond)
        {
            var sanction = await repositoryManager.SanctionRepository.GetEntityByConditionWithTrackingAsync(d => !d.IsDeleted && d.Id == sanctiond) ??
                throw new BusinessValidationException(LeillaKeys.SorrySanctionNotFound);
            sanction.Delete();
            await unitOfWork.SaveAsync();
            return true;
        }
        public async Task<GetSanctionsInformationsResponseDTO> GetSanctionsInformations()
        {
            var sanctionRepository = repositoryManager.SanctionRepository;
            var query = sanctionRepository.Get(sanction => sanction.CompanyId == requestInfo.CompanyId);

            #region Handle Response

            return new GetSanctionsInformationsResponseDTO
            {
                TotalCount = await query.CountAsync(),
                ActiveCount = await query.Where(sanction => !sanction.IsDeleted && sanction.IsActive).CountAsync(),
                NotActiveCount = await query.Where(sanction => !sanction.IsDeleted && !sanction.IsActive).CountAsync(),
                DeletedCount = await query.Where(sanction => sanction.IsDeleted).CountAsync()
            };

            #endregion
        }
    }
}

