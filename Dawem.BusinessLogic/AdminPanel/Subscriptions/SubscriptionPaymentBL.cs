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
using Dawem.Models.Dtos.Dawem.Subscriptions.SubscriptionPayment;
using Dawem.Models.DTOs.Dawem.Generic.Exceptions;
using Dawem.Models.Response.AdminPanel.Subscriptions.SubscriptionPayment;
using Dawem.Translations;
using Microsoft.EntityFrameworkCore;

namespace Dawem.BusinessLogic.AdminPanel.Subscriptions
{
    public class SubscriptionPaymentBL : ISubscriptionPaymentBL
    {
        private readonly IUnitOfWork<ApplicationDBContext> unitOfWork;
        private readonly RequestInfo requestInfo;
        private readonly ISubscriptionPaymentBLValidation subscriptionPaymentBLValidation;
        private readonly IRepositoryManager repositoryManager;
        private readonly IMapper mapper;
        private readonly IUploadBLC uploadBLC;

        public SubscriptionPaymentBL(IUnitOfWork<ApplicationDBContext> _unitOfWork,
            IRepositoryManager _repositoryManager,
            IMapper _mapper,
           RequestInfo _requestHeaderContext,
           IUploadBLC _uploadBLC,
           ISubscriptionPaymentBLValidation _subscriptionPaymentBLValidation)
        {
            unitOfWork = _unitOfWork;
            requestInfo = _requestHeaderContext;
            repositoryManager = _repositoryManager;
            subscriptionPaymentBLValidation = _subscriptionPaymentBLValidation;
            mapper = _mapper;
            uploadBLC = _uploadBLC;

        }
        public async Task<int> Create(CreateSubscriptionPaymentModel model)
        {
            #region Business Validation

            await subscriptionPaymentBLValidation.CreateValidation(model);

            #endregion

            unitOfWork.CreateTransaction();

            #region Insert SubscriptionPayment

            #region Set SubscriptionPayment code
            var getNextCode = await repositoryManager.SubscriptionPaymentRepository
                .Get()
                .Select(e => e.Code)
                .DefaultIfEmpty()
                .MaxAsync() + 1;
            #endregion

            var subscriptionPayment = mapper.Map<SubscriptionPayment>(model);
            subscriptionPayment.AddUserId = requestInfo.UserId;
            subscriptionPayment.AddedApplicationType = requestInfo.ApplicationType;
            subscriptionPayment.Code = getNextCode;
            repositoryManager.SubscriptionPaymentRepository.Insert(subscriptionPayment);

            await unitOfWork.SaveAsync();

            #endregion

            #region Handle Response

            await unitOfWork.CommitAsync();
            return subscriptionPayment.Id;

            #endregion

        }
        public async Task<bool> Update(UpdateSubscriptionPaymentModel model)
        {
            #region Business Validation

            await subscriptionPaymentBLValidation.UpdateValidation(model);

            #endregion

            unitOfWork.CreateTransaction();

            #region Update SubscriptionPayment

            var getSubscriptionPayment = await repositoryManager.SubscriptionPaymentRepository.GetEntityByConditionWithTrackingAsync(subscriptionPayment => !subscriptionPayment.IsDeleted
            && subscriptionPayment.Id == model.Id) ?? throw new BusinessValidationException(LeillaKeys.SorrySubscriptionPaymentNotFound);

            getSubscriptionPayment.SubscriptionId = model.SubscriptionId;
            getSubscriptionPayment.IsActive = model.IsActive;
            getSubscriptionPayment.ModifiedDate = DateTime.Now;
            getSubscriptionPayment.ModifyUserId = requestInfo.UserId;
            getSubscriptionPayment.Amount = model.Amount;
            getSubscriptionPayment.Date = model.Date;
            getSubscriptionPayment.Notes = model.Notes;

            await unitOfWork.SaveAsync();

            #endregion

            #region Handle Response

            await unitOfWork.CommitAsync();
            return true;

            #endregion
        }
        public async Task<GetSubscriptionPaymentsResponse> Get(GetSubscriptionPaymentsCriteria criteria)
        {
            var subscriptionPaymentRepository = repositoryManager.SubscriptionPaymentRepository;
            var query = subscriptionPaymentRepository.GetAsQueryable(criteria);

            #region paging
            int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            int take = PagingHelper.Take(criteria.PageSize);
            #region sorting
            var queryOrdered = subscriptionPaymentRepository.OrderBy(query, nameof(SubscriptionPayment.Id), LeillaKeys.Desc);
            #endregion
            var queryPaged = criteria.GetPagingEnabled() ? queryOrdered.Skip(skip).Take(take) : queryOrdered;
            #endregion

            #region Handle Response

            var subscriptionPaymentsList = await queryPaged.Select(subscriptionPayment => new GetSubscriptionPaymentsResponseModel
            {
                Id = subscriptionPayment.Id,
                Code = subscriptionPayment.Code,
                SubscriptionInfo = subscriptionPayment.Subscription.Code + LeillaKeys.Dash + subscriptionPayment.Subscription.Company.Name
                + LeillaKeys.Dash + subscriptionPayment.Subscription.Plan.PlanNameTranslations.FirstOrDefault(p => p.Language.ISO2 == requestInfo.Lang).Name,
                Amount = subscriptionPayment.Amount,
                IsActive = subscriptionPayment.IsActive
            }).ToListAsync();

            return new GetSubscriptionPaymentsResponse
            {
                SubscriptionPayments = subscriptionPaymentsList,
                TotalCount = await query.CountAsync()
            };
            #endregion

        }
        public async Task<GetSubscriptionPaymentInfoResponseModel> GetInfo(int subscriptionPaymentId)
        {
            var subscriptionPayment = await repositoryManager.SubscriptionPaymentRepository.Get(e => e.Id == subscriptionPaymentId && !e.IsDeleted)
                .Select(subscriptionPayment => new GetSubscriptionPaymentInfoResponseModel
                {
                    Code = subscriptionPayment.Code,
                    SubscriptionInfo = subscriptionPayment.Subscription.Code + LeillaKeys.Dash + subscriptionPayment.Subscription.Company.Name
                    + LeillaKeys.Dash + subscriptionPayment.Subscription.Plan.PlanNameTranslations.FirstOrDefault(p => p.Language.ISO2 == requestInfo.Lang).Name,
                    Amount = subscriptionPayment.Amount,
                    Date = subscriptionPayment.Date,
                    IsActive = subscriptionPayment.IsActive,
                    Notes = subscriptionPayment.Notes
                }).FirstOrDefaultAsync() ?? throw new BusinessValidationException(LeillaKeys.SorrySubscriptionPaymentNotFound);

            return subscriptionPayment;
        }
        public async Task<GetSubscriptionPaymentByIdResponseModel> GetById(int subscriptionPaymentId)
        {
            var subscriptionPayment = await repositoryManager.SubscriptionPaymentRepository.Get(e => e.Id == subscriptionPaymentId && !e.IsDeleted)
                .Select(subscriptionPayment => new GetSubscriptionPaymentByIdResponseModel
                {
                    Id = subscriptionPayment.Id,
                    Code = subscriptionPayment.Code,
                    SubscriptionId = subscriptionPayment.SubscriptionId,
                    Amount = subscriptionPayment.Amount,
                    Date = subscriptionPayment.Date,
                    IsActive = subscriptionPayment.IsActive,
                    Notes = subscriptionPayment.Notes
                }).FirstOrDefaultAsync() ?? throw new BusinessValidationException(LeillaKeys.SorrySubscriptionPaymentNotFound);

            return subscriptionPayment;

        }
        public async Task<bool> Delete(int subscriptionPaymentd)
        {
            var subscriptionPayment = await repositoryManager.SubscriptionPaymentRepository.GetEntityByConditionWithTrackingAsync(d => !d.IsDeleted && d.Id == subscriptionPaymentd) ??
                throw new BusinessValidationException(LeillaKeys.SorrySubscriptionPaymentNotFound);
            subscriptionPayment.Delete();
            await unitOfWork.SaveAsync();
            return true;
        }
        public async Task<bool> Enable(int subscriptionPaymentId)
        {
            var subscriptionPayment = await repositoryManager.SubscriptionPaymentRepository.GetEntityByConditionWithTrackingAsync(d => !d.IsDeleted && !d.IsActive && d.Id == subscriptionPaymentId) ??
                throw new BusinessValidationException(LeillaKeys.SorrySubscriptionPaymentNotFound);
            subscriptionPayment.Enable();
            await unitOfWork.SaveAsync();
            return true;
        }
        public async Task<bool> Disable(DisableModelDTO model)
        {
            var group = await repositoryManager.SubscriptionPaymentRepository.GetEntityByConditionWithTrackingAsync(d => !d.IsDeleted && d.IsActive && d.Id == model.Id) ??
                throw new BusinessValidationException(LeillaKeys.SorrySubscriptionPaymentNotFound);
            group.Disable(model.DisableReason);
            await unitOfWork.SaveAsync();
            return true;
        }
        public async Task<GetSubscriptionPaymentsInformationsResponseDTO> GetSubscriptionPaymentsInformations()
        {
            var subscriptionPaymentRepository = repositoryManager.SubscriptionPaymentRepository;
            var query = subscriptionPaymentRepository.Get();

            #region Handle Response

            return new GetSubscriptionPaymentsInformationsResponseDTO
            {
                TotalCount = await query.Where(subscriptionPayment => !subscriptionPayment.IsDeleted).CountAsync(),
                ActiveCount = await query.Where(subscriptionPayment => !subscriptionPayment.IsDeleted && subscriptionPayment.IsActive).CountAsync(),
                NotActiveCount = await query.Where(subscriptionPayment => !subscriptionPayment.IsDeleted && !subscriptionPayment.IsActive).CountAsync(),
                DeletedCount = await query.Where(subscriptionPayment => subscriptionPayment.IsDeleted).CountAsync()
            };

            #endregion
        }
    }
}

