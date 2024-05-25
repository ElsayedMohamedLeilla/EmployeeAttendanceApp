using AutoMapper;
using Dawem.Contract.BusinessLogic.AdminPanel.Subscriptions;
using Dawem.Contract.BusinessLogicCore.Dawem;
using Dawem.Contract.BusinessValidation.AdminPanel.Subscriptions;
using Dawem.Contract.Repository.Manager;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Subscriptions;
using Dawem.Helpers;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Dawem.Employees.Employees;
using Dawem.Models.Dtos.Dawem.Shared;
using Dawem.Models.Dtos.Dawem.Subscriptions.Plans;
using Dawem.Models.DTOs.Dawem.Generic.Exceptions;
using Dawem.Models.Response.AdminPanel.Subscriptions.Plans;
using Dawem.Translations;
using Microsoft.EntityFrameworkCore;

namespace Dawem.BusinessLogic.AdminPanel.Subscriptions
{
    public class PlanBL : IPlanBL
    {
        private readonly IUnitOfWork<ApplicationDBContext> unitOfWork;
        private readonly RequestInfo requestInfo;
        private readonly IPlanBLValidation planBLValidation;
        private readonly IRepositoryManager repositoryManager;
        private readonly IMapper mapper;
        private readonly IUploadBLC uploadBLC;

        public PlanBL(IUnitOfWork<ApplicationDBContext> _unitOfWork,
            IRepositoryManager _repositoryManager,
            IMapper _mapper,
           RequestInfo _requestHeaderContext,
           IUploadBLC _uploadBLC,

           IPlanBLValidation _planBLValidation)
        {
            unitOfWork = _unitOfWork;
            requestInfo = _requestHeaderContext;
            repositoryManager = _repositoryManager;
            planBLValidation = _planBLValidation;
            mapper = _mapper;
            uploadBLC = _uploadBLC;

        }
        public async Task<int> Create(CreatePlanModel model)
        {
            #region Business Validation

            await planBLValidation.CreateValidation(model);

            #endregion

            unitOfWork.CreateTransaction();

            #region Insert Plan

            #region Set Plan code
            var getNextCode = await repositoryManager.PlanRepository
                .Get()
                .Select(e => e.Code)
                .DefaultIfEmpty()
                .MaxAsync() + 1;
            #endregion

            var plan = mapper.Map<Plan>(model);
            plan.AddUserId = requestInfo.UserId;
            plan.AddedApplicationType = requestInfo.ApplicationType;
            plan.Code = getNextCode;
            repositoryManager.PlanRepository.Insert(plan);

            await unitOfWork.SaveAsync();

            #endregion

            #region Handle Response

            await unitOfWork.CommitAsync();
            return plan.Id;

            #endregion

        }
        public async Task<bool> Update(UpdatePlanModel model)
        {
            #region Business Validation

            await planBLValidation.UpdateValidation(model);

            #endregion

            unitOfWork.CreateTransaction();

            #region Update Plan

            var getPlan = await repositoryManager.PlanRepository.GetEntityByConditionWithTrackingAsync(plan => !plan.IsDeleted
            && plan.Id == model.Id) ?? throw new BusinessValidationException(LeillaKeys.SorryPlanNotFound);

            getPlan.IsTrial = model.IsTrial;
            getPlan.IsActive = model.IsActive;
            getPlan.ModifiedDate = DateTime.Now;
            getPlan.ModifyUserId = requestInfo.UserId;
            getPlan.MinNumberOfEmployees = model.MinNumberOfEmployees;
            getPlan.MaxNumberOfEmployees = model.MaxNumberOfEmployees;
            getPlan.EmployeeCost = model.EmployeeCost;
            getPlan.Notes = model.Notes;
            getPlan.AllScreensAvailable = model.AllScreensAvailable;

            await unitOfWork.SaveAsync();

            #region Handle Update Name Translations

            var exisNameTranslationsDbList = await repositoryManager.PlanNameTranslationRepository
                    .Get(e => e.PlanId == getPlan.Id)
                    .ToListAsync();

            var existingNameTranslationsIds = exisNameTranslationsDbList.Select(e => e.Id)
                .ToList();

            var addedPlanNameTranslations = model.NameTranslations != null ? model.NameTranslations
                .Where(ge => !existingNameTranslationsIds.Contains(ge.Id))
                .Select(ge => new PlanNameTranslation
                {
                    PlanId = model.Id,
                    LanguageId = ge.LanguageId,
                    Name = ge.Name,
                    ModifyUserId = requestInfo.UserId,
                    ModifiedDate = DateTime.UtcNow
                }).ToList() : new List<PlanNameTranslation>();

            var removedPlanNameTranslationsIds = exisNameTranslationsDbList
                .Where(ge => model.NameTranslations == null || !model.NameTranslations.Select(i => i.Id).Contains(ge.Id))
                .Select(ge => ge.Id)
                .ToList();

            var removedPlanNameTranslations = exisNameTranslationsDbList
                .Where(e => removedPlanNameTranslationsIds.Contains(e.Id))
                .ToList();

            var updatedPlanNameTranslations = exisNameTranslationsDbList.
                Where(nt => model.NameTranslations != null && model.NameTranslations.
                Any(mi => mi.Id == nt.Id && (mi.Name != nt.Name || mi.LanguageId != nt.LanguageId))).
                ToList();

            if (removedPlanNameTranslations.Count() > 0)
                repositoryManager.PlanNameTranslationRepository.BulkDeleteIfExist(removedPlanNameTranslations);
            if (addedPlanNameTranslations.Count() > 0)
                repositoryManager.PlanNameTranslationRepository.BulkInsert(addedPlanNameTranslations);
            if (updatedPlanNameTranslations.Count() > 0)
            {
                updatedPlanNameTranslations.ForEach(i =>
                {
                    i.Name = model.NameTranslations.FirstOrDefault(mi => mi.Id == i.Id)?.Name;
                    i.LanguageId = model.NameTranslations.FirstOrDefault(mi => mi.Id == i.Id)?.LanguageId ?? 0;
                });
                repositoryManager.PlanNameTranslationRepository.BulkUpdate(updatedPlanNameTranslations);
            }

            #endregion

            #region Update Plan Screens 

            var existDbList = await repositoryManager.PlanScreenRepository
                    .Get(e => e.PlanId == getPlan.Id)
                    .ToListAsync();

            var existingScreenIds = existDbList.Select(e => e.ScreenId).ToList();

            var addedScreens = model.ScreenIds
                .Where(screenId => !existingScreenIds.Contains(screenId))
                .Select(screenId => new PlanScreen
                {
                    PlanId = getPlan.Id,
                    ScreenId = screenId,
                    ModifyUserId = requestInfo.UserId,
                    ModifiedDate = DateTime.UtcNow
                }).ToList();

            var removedScreenIds = existDbList
                .Where(ge => !model.ScreenIds.Contains(ge.ScreenId))
                .Select(ge => ge.ScreenId)
                .ToList();

            var removedScreens = await repositoryManager.PlanScreenRepository
                .Get(e => e.PlanId == model.Id && removedScreenIds.Contains(e.ScreenId))
                .ToListAsync();

            if (removedScreens.Count > 0)
                repositoryManager.PlanScreenRepository.BulkDeleteIfExist(removedScreens);
            if (addedScreens.Count > 0)
                repositoryManager.PlanScreenRepository.BulkInsert(addedScreens);

            await unitOfWork.SaveAsync();

            #endregion

            #endregion

            #region Handle Response

            await unitOfWork.CommitAsync();
            return true;

            #endregion
        }
        public async Task<GetPlansResponse> Get(GetPlansCriteria criteria)
        {
            var planRepository = repositoryManager.PlanRepository;
            var query = planRepository.GetAsQueryable(criteria);

            #region paging
            int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            int take = PagingHelper.Take(criteria.PageSize);
            #region sorting
            var queryOrdered = planRepository.OrderBy(query, nameof(Plan.Id), LeillaKeys.Desc);
            #endregion
            var queryPaged = criteria.GetPagingEnabled() ? queryOrdered.Skip(skip).Take(take) : queryOrdered;
            #endregion

            #region Handle Response

            var plansList = await queryPaged.Select(plan => new GetPlansResponseModel
            {
                Id = plan.Id,
                Code = plan.Code,
                Name = plan.PlanNameTranslations.FirstOrDefault(p => p.Language.ISO2 == requestInfo.Lang).Name,
                EmployeeCost = plan.EmployeeCost,
                IsTrial = plan.IsTrial,
                IsActive = plan.IsActive,
                SubscriptionsCount = plan.Subscriptions.Count
            }).ToListAsync();

            return new GetPlansResponse
            {
                Plans = plansList,
                TotalCount = await query.CountAsync()
            };
            #endregion

        }
        public async Task<GetPlansForDropDownResponse> GetForDropDown(GetPlansCriteria criteria)
        {
            criteria.IsActive = true;
            var planRepository = repositoryManager.PlanRepository;
            var query = planRepository.GetAsQueryable(criteria);

            #region paging

            int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            int take = PagingHelper.Take(criteria.PageSize);

            #region sorting
            var queryOrdered = planRepository.OrderBy(query, nameof(Plan.Id), LeillaKeys.Desc);
            #endregion

            var queryPaged = criteria.GetPagingEnabled() ? queryOrdered.Skip(skip).Take(take) : queryOrdered;

            #endregion

            #region Handle Response

            var plansList = await queryPaged.Select(plan => new GetPlansForDropDownResponseModel
            {
                Id = plan.Id,
                Name = plan.PlanNameTranslations.FirstOrDefault(p => p.Language.ISO2 == requestInfo.Lang).Name,
            }).ToListAsync();

            return new GetPlansForDropDownResponse
            {
                Plans = plansList,
                TotalCount = await query.CountAsync()
            };

            #endregion

        }
        public async Task<GetPlanInfoResponseModel> GetInfo(int planId)
        {
            var plan = await repositoryManager.PlanRepository.Get(e => e.Id == planId && !e.IsDeleted)
                .Select(plan => new GetPlanInfoResponseModel
                {
                    Code = plan.Code,
                    Name = plan.PlanNameTranslations.FirstOrDefault(p => p.Language.ISO2 == requestInfo.Lang).Name,
                    IsTrial = plan.IsTrial,
                    MinNumberOfEmployees = plan.MinNumberOfEmployees,
                    MaxNumberOfEmployees = plan.MaxNumberOfEmployees,
                    EmployeeCost = plan.EmployeeCost,
                    IsActive = plan.IsActive,
                    Notes = plan.Notes,
                    SubscriptionsCount = plan.Subscriptions.Count,
                    AllScreensAvailable = plan.AllScreensAvailable,
                    Screens = plan.PlanScreens != null ? plan.PlanScreens.Select(s => s.Screen.
                    ScreenNameTranslations.
                    FirstOrDefault(p => p.Language.ISO2 == requestInfo.Lang).Name).
                    ToList() : null,
                    NameTranslations = plan.PlanNameTranslations.
                    Select(pt =>
                    new NameTranslationGetInfoModel
                    {
                        Name = pt.Name,
                        LanguageName = pt.Language.NativeName
                    }).ToList()
                }).FirstOrDefaultAsync() ?? throw new BusinessValidationException(LeillaKeys.SorryPlanNotFound);

            return plan;
        }
        public async Task<GetPlanByIdResponseModel> GetById(int planId)
        {
            var plan = await repositoryManager.PlanRepository.Get(e => e.Id == planId && !e.IsDeleted)
                .Select(plan => new GetPlanByIdResponseModel
                {
                    Id = plan.Id,
                    Code = plan.Code,
                    IsTrial = plan.IsTrial,
                    MinNumberOfEmployees = plan.MinNumberOfEmployees,
                    MaxNumberOfEmployees = plan.MaxNumberOfEmployees,
                    EmployeeCost = plan.EmployeeCost,
                    IsActive = plan.IsActive,
                    AllScreensAvailable = plan.AllScreensAvailable,
                    ScreenIds = plan.PlanScreens != null ? plan.PlanScreens.Select(s => s.ScreenId).ToList() : null,
                    Notes = plan.Notes,
                    NameTranslations = plan.PlanNameTranslations.
                    Select(pt => new NameTranslationModel
                    {
                        Id = pt.Id,
                        Name = pt.Name,
                        LanguageId = pt.LanguageId
                    }).ToList()
                }).FirstOrDefaultAsync() ?? throw new BusinessValidationException(LeillaKeys.SorryPlanNotFound);

            return plan;

        }
        public async Task<bool> Delete(int pland)
        {
            var plan = await repositoryManager.PlanRepository.GetEntityByConditionWithTrackingAsync(d => !d.IsDeleted && d.Id == pland) ??
                throw new BusinessValidationException(LeillaKeys.SorryPlanNotFound);
            plan.Delete();
            await unitOfWork.SaveAsync();
            return true;
        }
        public async Task<bool> Enable(int planId)
        {
            var plan = await repositoryManager.PlanRepository.GetEntityByConditionWithTrackingAsync(d => !d.IsDeleted && !d.IsActive && d.Id == planId) ??
                throw new BusinessValidationException(LeillaKeys.SorryPlanNotFound);
            plan.Enable();
            await unitOfWork.SaveAsync();
            return true;
        }
        public async Task<bool> Disable(DisableModelDTO model)
        {
            var group = await repositoryManager.PlanRepository.GetEntityByConditionWithTrackingAsync(d => !d.IsDeleted && d.IsActive && d.Id == model.Id) ??
                throw new BusinessValidationException(LeillaKeys.SorryPlanNotFound);
            group.Disable(model.DisableReason);
            await unitOfWork.SaveAsync();
            return true;
        }
        public async Task<GetPlansInformationsResponseDTO> GetPlansInformations()
        {
            var planRepository = repositoryManager.PlanRepository;
            var query = planRepository.Get();

            #region Handle Response

            return new GetPlansInformationsResponseDTO
            {
                TotalCount = await query.Where(plan => !plan.IsDeleted).CountAsync(),
                ActiveCount = await query.Where(plan => !plan.IsDeleted && plan.IsActive).CountAsync(),
                NotActiveCount = await query.Where(plan => !plan.IsDeleted && !plan.IsActive).CountAsync(),
                DeletedCount = await query.Where(plan => plan.IsDeleted).CountAsync()
            };

            #endregion
        }
    }
}

