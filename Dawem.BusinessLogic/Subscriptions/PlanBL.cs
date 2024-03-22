using AutoMapper;
using Dawem.Contract.BusinessLogic.Subscriptions;
using Dawem.Contract.BusinessLogicCore;
using Dawem.Contract.BusinessValidation.Subscriptions;
using Dawem.Contract.Repository.Manager;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Subscriptions;
using Dawem.Helpers;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Employees.Employees;
using Dawem.Models.Dtos.Subscriptions.Plans;
using Dawem.Models.Exceptions;
using Dawem.Models.Response.Subscriptions.Plans;
using Dawem.Translations;
using Microsoft.EntityFrameworkCore;

namespace Dawem.BusinessLogic.Subscriptions
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
            && plan.Id == model.Id);

            if (getPlan != null)
            {
                getPlan.NameAr = model.NameAr;
                getPlan.NameEn = model.NameEn;
                getPlan.IsTrial = model.IsTrial;
                getPlan.IsActive = model.IsActive;
                getPlan.ModifiedDate = DateTime.Now;
                getPlan.ModifyUserId = requestInfo.UserId;
                getPlan.MinNumberOfEmployees = model.MinNumberOfEmployees;
                getPlan.MaxNumberOfEmployees = model.MaxNumberOfEmployees;
                getPlan.EmployeeCost = model.EmployeeCost;
                getPlan.GracePeriodPercentage = model.GracePeriodPercentage;
                getPlan.Notes = model.Notes;

                await unitOfWork.SaveAsync();

                #region Handle Response
                await unitOfWork.CommitAsync();
                return true;
                #endregion
            }
            #endregion

            else
                throw new BusinessValidationException(LeillaKeys.SorryPlanNotFound);


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
                NameAr = plan.NameAr,
                NameEn = plan.NameEn,
                EmployeeCost = plan.EmployeeCost,
                IsTrial = plan.IsTrial,
                IsActive = plan.IsActive
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

            var plansList = await queryPaged.Select(e => new GetPlansForDropDownResponseModel
            {
                Id = e.Id,
                Name = e.NameAr
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
                    NameAr = plan.NameAr,
                    NameEn = plan.NameEn,
                    IsTrial = plan.IsTrial,
                    MinNumberOfEmployees = plan.MinNumberOfEmployees,
                    MaxNumberOfEmployees = plan.MaxNumberOfEmployees,
                    EmployeeCost = plan.EmployeeCost,
                    GracePeriodPercentage = plan.GracePeriodPercentage,
                    IsActive = plan.IsActive,
                    Notes = plan.Notes
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
                    NameAr = plan.NameAr,
                    NameEn = plan.NameEn,
                    IsTrial = plan.IsTrial,
                    MinNumberOfEmployees = plan.MinNumberOfEmployees,
                    MaxNumberOfEmployees = plan.MaxNumberOfEmployees,
                    EmployeeCost = plan.EmployeeCost,
                    GracePeriodPercentage = plan.GracePeriodPercentage,
                    IsActive = plan.IsActive,
                    Notes = plan.Notes
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

