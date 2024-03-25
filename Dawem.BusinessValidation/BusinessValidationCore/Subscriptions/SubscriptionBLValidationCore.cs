using Dawem.Contract.BusinessValidationCore.Subscriptions;
using Dawem.Contract.Repository.Manager;
using Dawem.Enums.Generals;
using Dawem.Models.Context;
using Dawem.Models.Criteria.Subscriptions;
using Dawem.Models.Exceptions;
using Dawem.Models.Response.Subscriptions;
using Dawem.Translations;


namespace Dawem.Validation.BusinessValidationCore.Subscriptions
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
            var dawemSettingRepository = repositoryManager.DawemSettingRepository;
            var result = new CheckCompanySubscriptionResponseModel();

            var getSubscription = await subscriptionRepository.
                GetEntityByConditionAsync(s => s.CompanyId == model.CompanyId);

            if (getSubscription != null)
            {
                var isSubscriptionExpired = false;
                if (DateTime.Now.Date >= getSubscription.EndDate.Date)
                {
                    var getPlansGracePeriodPercentage = (await dawemSettingRepository.
                        GetEntityByConditionAsync(d => !d.IsDeleted && d.Type == DawemSettingType.PlansGracePeriodPercentage))?.
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

            return result;

        }
    }
}
