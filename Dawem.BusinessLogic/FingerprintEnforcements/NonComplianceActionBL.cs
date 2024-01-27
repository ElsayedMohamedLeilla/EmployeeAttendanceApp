using AutoMapper;
using Dawem.Contract.BusinessLogic.Employees;
using Dawem.Contract.BusinessValidation.Employees;
using Dawem.Contract.Repository.Manager;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Employees;
using Dawem.Helpers;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Employees.Employees;
using Dawem.Models.Dtos.FingerprintEnforcements.NonComplianceActions;
using Dawem.Models.Exceptions;
using Dawem.Models.Response.Employees.AssignmentTypes;
using Dawem.Translations;
using Microsoft.EntityFrameworkCore;

namespace Dawem.BusinessLogic.Employees
{
    public class NonComplianceActionBL : INonComplianceActionBL
    {
        private readonly IUnitOfWork<ApplicationDBContext> unitOfWork;
        private readonly RequestInfo requestInfo;
        private readonly INonComplianceActionBLValidation nonComplianceActionBLValidation;
        private readonly IRepositoryManager repositoryManager;
        private readonly IMapper mapper;
        public NonComplianceActionBL(IUnitOfWork<ApplicationDBContext> _unitOfWork,
            IRepositoryManager _repositoryManager,
            IMapper _mapper,
           RequestInfo _requestHeaderContext,
           INonComplianceActionBLValidation _nonComplianceActionBLValidation)
        {
            unitOfWork = _unitOfWork;
            requestInfo = _requestHeaderContext;
            repositoryManager = _repositoryManager;
            nonComplianceActionBLValidation = _nonComplianceActionBLValidation;
            mapper = _mapper;
        }
        public async Task<int> Create(CreateNonComplianceActionModel model)
        {
            #region Business Validation

            await nonComplianceActionBLValidation.CreateValidation(model);

            #endregion

            unitOfWork.CreateTransaction();

            #region Insert NonComplianceAction

            #region Set NonComplianceAction code
            var getNextCode = await repositoryManager.NonComplianceActionRepository
                .Get(e => e.CompanyId == requestInfo.CompanyId)
                .Select(e => e.Code)
                .DefaultIfEmpty()
                .MaxAsync() + 1;
            #endregion

            var nonComplianceAction = mapper.Map<NonComplianceAction>(model);
            nonComplianceAction.CompanyId = requestInfo.CompanyId;
            nonComplianceAction.AddUserId = requestInfo.UserId;

            nonComplianceAction.Code = getNextCode;
            repositoryManager.NonComplianceActionRepository.Insert(nonComplianceAction);
            await unitOfWork.SaveAsync();

            #endregion

            #region Handle Response

            await unitOfWork.CommitAsync();
            return nonComplianceAction.Id;

            #endregion

        }
        public async Task<bool> Update(UpdateNonComplianceActionModel model)
        {
            #region Business Validation

            await nonComplianceActionBLValidation.UpdateValidation(model);

            #endregion

            unitOfWork.CreateTransaction();

            #region Update NonComplianceAction

            var getNonComplianceAction = await repositoryManager.NonComplianceActionRepository
                .GetEntityByConditionWithTrackingAsync(nonComplianceAction => !nonComplianceAction.IsDeleted
                && nonComplianceAction.Id == model.Id);


            if (getNonComplianceAction != null)
            {
                getNonComplianceAction.Name = model.Name;
                getNonComplianceAction.IsActive = model.IsActive;
                getNonComplianceAction.ModifiedDate = DateTime.Now;
                getNonComplianceAction.ModifyUserId = requestInfo.UserId;
                await unitOfWork.SaveAsync();
                #region Handle Response
                await unitOfWork.CommitAsync();
                return true;
                #endregion
            }
            #endregion

            else
                throw new BusinessValidationException(LeillaKeys.SorryNonComplianceActionNotFound);


        }
        public async Task<GetNonComplianceActionsResponse> Get(GetNonComplianceActionsCriteria criteria)
        {
            var nonComplianceActionRepository = repositoryManager.NonComplianceActionRepository;
            var query = nonComplianceActionRepository.GetAsQueryable(criteria);

            #region paging
            int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            int take = PagingHelper.Take(criteria.PageSize);
            #region sorting
            var queryOrdered = nonComplianceActionRepository.OrderBy(query, nameof(NonComplianceAction.Id), LeillaKeys.Desc);
            #endregion
            var queryPaged = criteria.PagingEnabled ? queryOrdered.Skip(skip).Take(take) : queryOrdered;
            #endregion

            #region Handle Response

            var nonComplianceActionsList = await queryPaged.Select(e => new GetNonComplianceActionsResponseModel
            {
                Id = e.Id,
                Code = e.Code,
                Name = e.Name,
                TypeName = TranslationHelper.GetTranslation(e.Type.ToString() + LeillaKeys.NonComplianceActionType, requestInfo.Lang),
                IsActive = e.IsActive,
            }).ToListAsync();
            return new GetNonComplianceActionsResponse
            {
                NonComplianceActions = nonComplianceActionsList,
                TotalCount = await query.CountAsync()
            };
            #endregion

        }
        public async Task<GetNonComplianceActionsForDropDownResponse> GetForDropDown(GetNonComplianceActionsCriteria criteria)
        {

            criteria.IsActive = true;
            var nonComplianceActionRepository = repositoryManager.NonComplianceActionRepository;
            var query = nonComplianceActionRepository.GetAsQueryable(criteria);

            #region paging

            int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            int take = PagingHelper.Take(criteria.PageSize);

            #region sorting
            var queryOrdered = nonComplianceActionRepository.OrderBy(query, nameof(NonComplianceAction.Id), LeillaKeys.Desc);
            #endregion

            var queryPaged = criteria.PagingEnabled ? queryOrdered.Skip(skip).Take(take) : queryOrdered;

            #endregion

            #region Handle Response

            var nonComplianceActionsList = await queryPaged.Select(e => new GetNonComplianceActionsForDropDownResponseModel
            {
                Id = e.Id,
                Name = e.Name
            }).ToListAsync();

            return new GetNonComplianceActionsForDropDownResponse
            {
                NonComplianceActions = nonComplianceActionsList,
                TotalCount = await query.CountAsync()
            };

            #endregion

        }
        public async Task<GetNonComplianceActionInfoResponseModel> GetInfo(int NonComplianceActionId)
        {
            var nonComplianceAction = await repositoryManager.NonComplianceActionRepository.Get(e => e.Id == NonComplianceActionId && !e.IsDeleted)
                .Select(e => new GetNonComplianceActionInfoResponseModel
                {
                    Code = e.Code,
                    Name = e.Name,
                    Type = e.Type,
                    TypeName = TranslationHelper.GetTranslation(e.Type.ToString() + LeillaKeys.NonComplianceActionType, requestInfo.Lang),
                    WarningMessage = e.WarningMessage,
                    IsActive = e.IsActive,
                }).FirstOrDefaultAsync() ?? throw new BusinessValidationException(LeillaKeys.SorryNonComplianceActionNotFound);

            return nonComplianceAction;
        }
        public async Task<GetNonComplianceActionByIdResponseModel> GetById(int NonComplianceActionId)
        {
            var nonComplianceAction = await repositoryManager.NonComplianceActionRepository.Get(e => e.Id == NonComplianceActionId && !e.IsDeleted)
                .Select(e => new GetNonComplianceActionByIdResponseModel
                {
                    Id = e.Id,
                    Code = e.Code,
                    Name = e.Name,
                    Type = e.Type,
                    WarningMessage = e.WarningMessage,
                    IsActive = e.IsActive,
                }).FirstOrDefaultAsync() ?? throw new BusinessValidationException(LeillaKeys.SorryNonComplianceActionNotFound);

            return nonComplianceAction;

        }
        public async Task<bool> Enable(int nonComplianceActiond)
        {
            var nonComplianceAction = await repositoryManager.NonComplianceActionRepository.GetEntityByConditionWithTrackingAsync(d => !d.IsDeleted && !d.IsActive && d.Id == nonComplianceActiond) ??
                throw new BusinessValidationException(LeillaKeys.SorryNonComplianceActionNotFound);
            nonComplianceAction.Enable();
            await unitOfWork.SaveAsync();
            return true;
        }
        public async Task<bool> Disable(DisableModelDTO model)
        {
            var nonComplianceAction = await repositoryManager.NonComplianceActionRepository.GetEntityByConditionWithTrackingAsync(d => !d.IsDeleted && d.IsActive && d.Id == model.Id) ??
                throw new BusinessValidationException(LeillaKeys.SorryNonComplianceActionNotFound);
            nonComplianceAction.Disable(model.DisableReason);
            await unitOfWork.SaveAsync();
            return true;
        }
        public async Task<bool> Delete(int nonComplianceActiond)
        {
            var nonComplianceAction = await repositoryManager.NonComplianceActionRepository.GetEntityByConditionWithTrackingAsync(d => !d.IsDeleted && d.Id == nonComplianceActiond) ??
                throw new BusinessValidationException(LeillaKeys.SorryNonComplianceActionNotFound);
            nonComplianceAction.Delete();
            await unitOfWork.SaveAsync();
            return true;
        }
        public async Task<GetNonComplianceActionsInformationsResponseDTO> GetNonComplianceActionsInformations()
        {
            var nonComplianceActionRepository = repositoryManager.NonComplianceActionRepository;
            var query = nonComplianceActionRepository.Get(nonComplianceAction => nonComplianceAction.CompanyId == requestInfo.CompanyId);

            #region Handle Response

            return new GetNonComplianceActionsInformationsResponseDTO
            {
                TotalCount = await query.Where(nonComplianceAction => !nonComplianceAction.IsDeleted).CountAsync(),
                ActiveCount = await query.Where(nonComplianceAction => !nonComplianceAction.IsDeleted && nonComplianceAction.IsActive).CountAsync(),
                NotActiveCount = await query.Where(nonComplianceAction => !nonComplianceAction.IsDeleted && !nonComplianceAction.IsActive).CountAsync(),
                DeletedCount = await query.Where(nonComplianceAction => nonComplianceAction.IsDeleted).CountAsync()
            };

            #endregion
        }
    }
}

