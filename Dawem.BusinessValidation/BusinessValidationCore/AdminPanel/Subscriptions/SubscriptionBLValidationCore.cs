﻿using Dawem.Contract.BusinessValidationCore.AdminPanel.Subscriptions;
using Dawem.Contract.Repository.Manager;
using Dawem.Enums.Generals;
using Dawem.Models.Context;
using Dawem.Models.Criteria.Subscriptions;
using Dawem.Models.DTOs.Dawem.Generic.Exceptions;
using Dawem.Models.Response.AdminPanel.Subscriptions;
using Dawem.Translations;
using Microsoft.EntityFrameworkCore;


namespace Dawem.Validation.BusinessValidationCore.AdminPanel.Subscriptions
{

    public class SubscriptionBLValidationCore : ISubscriptionBLValidationCore
    {
        private readonly IRepositoryManager repositoryManager;
        private readonly RequestInfo requestInfo;
        public SubscriptionBLValidationCore(IRepositoryManager _repositoryManager, RequestInfo _requestInfo)
        {
            repositoryManager = _repositoryManager;
            requestInfo = _requestInfo;
        }
        public async Task<CheckCompanySubscriptionResponseModel> CheckCompanySubscription(CheckCompanySubscriptionModel model)
        {
            var subscriptionRepository = repositoryManager.SubscriptionRepository;
            var companyRepository = repositoryManager.CompanyRepository;
            var dawemSettingRepository = repositoryManager.SettingRepository;
            var result = new CheckCompanySubscriptionResponseModel();

            var checkCompanyStaus = await companyRepository.
                Get(s => s.Id == model.CompanyId && !s.IsActive).AnyAsync();

            if (checkCompanyStaus)
            {
                switch (model.FromType)
                {
                    case CheckCompanySubscriptionFromType.SubscriptionMiddleWare:

                        result.Result = false;
                        result.ErrorType = CheckCompanySubscriptionErrorType.CompanyNotActive;
                        break;

                    case CheckCompanySubscriptionFromType.LogIn:

                        throw new BusinessValidationException(LeillaKeys.SorryYourCompanyStatusIsNotActiveRightNowPleaseContactDawemSupportTeamForInquiry);

                    default:
                        break;
                }
            }
            else
            {
                var getSubscription = await subscriptionRepository.
                GetEntityByConditionAsync(s => s.CompanyId == model.CompanyId);

                if (getSubscription != null)
                {
                    var isSubscriptionExpired = false;
                    if (getSubscription.IsWaitingForApproval)
                    {
                        switch (model.FromType)
                        {
                            case CheckCompanySubscriptionFromType.SubscriptionMiddleWare:

                                result.Result = false;
                                result.ErrorType = CheckCompanySubscriptionErrorType.SubscriptionIsWaitingForApproval;

                                break;
                            case CheckCompanySubscriptionFromType.LogIn:

                                throw new BusinessValidationException(LeillaKeys.SorryYourSubscriptionStatusOnDawemIsWaitingForApprovalPleaseContactDawemSupportTeamForInquiry);

                            default:
                                break;
                        }
                    }
                    else
                    {
                        if (DateTime.Now.Date >= getSubscription.EndDate.Date)
                        {
                            var getPlansGracePeriodPercentage = (await dawemSettingRepository.
                                GetEntityByConditionAsync(d => !d.IsDeleted && d.SettingType == (int)AdminPanelSettingType.PlanGracePeriodPercentage))?.
                                Integer;

                            var extraDays = 0;

                            if (getPlansGracePeriodPercentage != null)
                            {
                                extraDays = getPlansGracePeriodPercentage.Value * getSubscription.DurationInDays / 100;
                            }

                            var newEndDate = getSubscription.EndDate.AddDays(extraDays).Date;

                            if (DateTime.Now.Date >= newEndDate)
                            {
                                switch (model.FromType)
                                {
                                    case CheckCompanySubscriptionFromType.SubscriptionMiddleWare:

                                        result.Result = false;
                                        result.ErrorType = CheckCompanySubscriptionErrorType.SubscriptionExpired;
                                        isSubscriptionExpired = true;

                                        break;
                                    case CheckCompanySubscriptionFromType.LogIn:

                                        throw new BusinessValidationException(LeillaKeys.SorryYourSubscriptionOnDawemIsExpiredPleaseContactDawemSupportTeamForRenewal);

                                    default:
                                        break;
                                }

                            }
                        }
                        if (getSubscription.Status != SubscriptionStatus.Active && !isSubscriptionExpired)
                        {
                            switch (model.FromType)
                            {
                                case CheckCompanySubscriptionFromType.SubscriptionMiddleWare:

                                    result.Result = false;
                                    result.ErrorType = CheckCompanySubscriptionErrorType.SubscriptionNotActive;

                                    break;
                                case CheckCompanySubscriptionFromType.LogIn:

                                    throw new BusinessValidationException(LeillaKeys.SorryYourSubscriptionIsNotActiveRightNowPleaseContactDawemSupportTeamForInquiry);

                                default:
                                    break;
                            }

                        }
                    }
                }
            }

            return result;

        }
    }
}
