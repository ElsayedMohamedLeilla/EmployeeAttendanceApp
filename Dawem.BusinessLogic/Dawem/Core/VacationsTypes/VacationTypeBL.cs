using AutoMapper;
using Dawem.Contract.BusinessLogic.Dawem.Core;
using Dawem.Contract.BusinessValidation.Dawem.Core;
using Dawem.Contract.Repository.Manager;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Helpers;
using Dawem.Models.Context;
using Dawem.Models.Criteria.Core;
using Dawem.Models.Dtos.Dawem.Core.VacationsTypes;
using Dawem.Models.Generic.Exceptions;
using Dawem.Models.Response.Dawem.Core.VacationsTypes;
using Dawem.Translations;
using Microsoft.EntityFrameworkCore;

namespace Dawem.BusinessLogic.Dawem.Core.VacationsTypes
{
    public class VacationTypeBL : IVacationTypeBL
    {
        private readonly IUnitOfWork<ApplicationDBContext> unitOfWork;
        private readonly RequestInfo requestInfo;
        private readonly IVacationTypeBLValidation vacationTypeBLValidation;
        private readonly IRepositoryManager repositoryManager;
        private readonly IMapper mapper;


        public VacationTypeBL(IUnitOfWork<ApplicationDBContext> _unitOfWork,
         IRepositoryManager _repositoryManager,
         IMapper _mapper,
        RequestInfo _requestHeaderContext,
        IVacationTypeBLValidation _VacationsTypeBLValidation)
        {
            unitOfWork = _unitOfWork;
            requestInfo = _requestHeaderContext;
            repositoryManager = _repositoryManager;
            vacationTypeBLValidation = _VacationsTypeBLValidation;
            mapper = _mapper;
        }
        public async Task<int> Create(CreateVacationsTypeDTO model)
        {
            #region Business Validation

            await vacationTypeBLValidation.CreateValidation(model);

            #endregion

            unitOfWork.CreateTransaction();

            #region Insert VacationsType

            #region Set VacationsType code

            var getNextCode = await repositoryManager.VacationsTypeRepository
                .Get(e => e.CompanyId == requestInfo.CompanyId)
                .Select(e => e.Code)
                .DefaultIfEmpty()
                .MaxAsync() + 1;

            #endregion

            var vacationType = mapper.Map<Domain.Entities.Core.VacationType>(model);
            vacationType.CompanyId = requestInfo.CompanyId;
            vacationType.AddUserId = requestInfo.UserId;
            vacationType.Code = getNextCode;
            repositoryManager.VacationsTypeRepository.Insert(vacationType);
            await unitOfWork.SaveAsync();

            #endregion

            #region Handle Response

            await unitOfWork.CommitAsync();
            return vacationType.Id;

            #endregion

        }
        public async Task<bool> Update(UpdateVacationsTypeDTO model)
        {
            #region Business Validation

            await vacationTypeBLValidation.UpdateValidation(model);

            #endregion

            unitOfWork.CreateTransaction();

            #region Update VacationsType
            var getVacationsType = await repositoryManager.VacationsTypeRepository.GetByIdAsync(model.Id);
            getVacationsType.Name = model.Name;
            getVacationsType.DefaultType = model.DefaultType;
            getVacationsType.IsActive = model.IsActive;
            getVacationsType.ModifiedDate = DateTime.Now;
            getVacationsType.ModifyUserId = requestInfo.UserId;
            await unitOfWork.SaveAsync();
            #endregion

            #region Handle Response

            await unitOfWork.CommitAsync();
            return true;

            #endregion
        }
        public async Task<GetVacationsTypeResponseDTO> Get(GetVacationsTypesCriteria criteria)
        {
            var VacationsTypeRepository = repositoryManager.VacationsTypeRepository;
            var query = VacationsTypeRepository.GetAsQueryable(criteria);

            #region paging

            int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            int take = PagingHelper.Take(criteria.PageSize);

            #region sorting

            var queryOrdered = VacationsTypeRepository.OrderBy(query, nameof(Domain.Entities.Core.VacationType.Id), LeillaKeys.Desc);

            #endregion

            var queryPaged = criteria.GetPagingEnabled() ? queryOrdered.Skip(skip).Take(take) : queryOrdered;

            #endregion

            #region Handle Response

            var VacationsTypesList = await queryPaged.Select(vacationType => new GetVacationsTypeResponseModelDTO
            {
                Id = vacationType.Id,
                Code = vacationType.Code,
                Name = vacationType.Name,
                DefaultType = vacationType.DefaultType,
                DefaultTypeName = TranslationHelper.GetTranslation(vacationType.DefaultType.ToString(), requestInfo.Lang),
                IsActive = vacationType.IsActive,
            }).ToListAsync();

            return new GetVacationsTypeResponseDTO
            {
                VacationsTypes = VacationsTypesList,
                TotalCount = await query.CountAsync()
            };

            #endregion

        }
        public async Task<GetVacationsTypeDropDownResponseDTO> GetForDropDown(GetVacationsTypesCriteria criteria)
        {
            criteria.IsActive = true;
            var VacationsTypeRepository = repositoryManager.VacationsTypeRepository;
            var query = VacationsTypeRepository.GetAsQueryable(criteria);

            #region paging

            int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            int take = PagingHelper.Take(criteria.PageSize);

            #region sorting

            var queryOrdered = VacationsTypeRepository.OrderBy(query, nameof(Domain.Entities.Core.VacationType.Id), LeillaKeys.Desc);

            #endregion

            var queryPaged = criteria.GetPagingEnabled() ? queryOrdered.Skip(skip).Take(take) : queryOrdered;

            #endregion

            #region Handle Response

            var VacationsTypesList = await queryPaged.Select(e => new GetVacationsTypeForDropDownResponseModelDTO
            {
                Id = e.Id,
                Name = e.Name
            }).ToListAsync();

            return new GetVacationsTypeDropDownResponseDTO
            {
                VacationsTypes = VacationsTypesList,
                TotalCount = await query.CountAsync()
            };

            #endregion

        }
        public async Task<GetVacationsTypeInfoResponseDTO> GetInfo(int VacationsTypeId)
        {
            var VacationsType = await repositoryManager.VacationsTypeRepository.Get(e => e.Id == VacationsTypeId && !e.IsDeleted)
                .Select(vacationType => new GetVacationsTypeInfoResponseDTO
                {
                    Code = vacationType.Code,
                    Name = vacationType.Name,
                    DefaultType = vacationType.DefaultType,
                    DefaultTypeName = TranslationHelper.GetTranslation(vacationType.DefaultType.ToString(), requestInfo.Lang),
                    IsActive = vacationType.IsActive,
                }).FirstOrDefaultAsync() ?? throw new BusinessValidationException(LeillaKeys.SorryVacationTypeNotFound);

            return VacationsType;
        }
        public async Task<GetVacationsTypeByIdResponseDTO> GetById(int VacationsTypeId)
        {
            var VacationsType = await repositoryManager.VacationsTypeRepository.Get(e => e.Id == VacationsTypeId && !e.IsDeleted)
                .Select(vacationType => new GetVacationsTypeByIdResponseDTO
                {
                    Id = vacationType.Id,
                    Code = vacationType.Code,
                    Name = vacationType.Name,
                    DefaultType = vacationType.DefaultType,
                    IsActive = vacationType.IsActive,

                }).FirstOrDefaultAsync() ?? throw new BusinessValidationException(LeillaKeys.SorryVacationTypeNotFound);

            return VacationsType;

        }
        public async Task<bool> Delete(int VacationsTypeId)
        {
            var vacationsType = await repositoryManager.VacationsTypeRepository.GetEntityByConditionWithTrackingAsync(d => !d.IsDeleted && d.Id == VacationsTypeId) ??
                throw new BusinessValidationException(LeillaKeys.SorryVacationTypeNotFound);
            vacationsType.Delete();

            await unitOfWork.SaveAsync();
            return true;
        }
        public async Task<GetVacationsTypesInformationsResponseDTO> GetVacationTypesInformations()
        {
            var vacationRepository = repositoryManager.VacationsTypeRepository;
            var query = vacationRepository.Get(vacation => vacation.CompanyId == requestInfo.CompanyId);

            #region Handle Response

            return new GetVacationsTypesInformationsResponseDTO
            {
                TotalCount = await query.Where(vacation => !vacation.IsDeleted).CountAsync(),
                ActiveCount = await query.Where(vacation => !vacation.IsDeleted && vacation.IsActive).CountAsync(),
                NotActiveCount = await query.Where(vacation => !vacation.IsDeleted && !vacation.IsActive).CountAsync(),
                DeletedCount = await query.Where(vacation => vacation.IsDeleted).CountAsync()
            };

            #endregion
        }
    }
}
