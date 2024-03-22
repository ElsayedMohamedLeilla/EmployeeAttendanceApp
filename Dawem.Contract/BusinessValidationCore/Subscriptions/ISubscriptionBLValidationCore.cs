using Dawem.Models.Criteria.Subscriptions;
using Dawem.Models.Response.Subscriptions;

namespace Dawem.Contract.BusinessValidationCore.Subscriptions
{
    public interface ISubscriptionBLValidationCore
    {
        Task<CheckCompanySubscriptionResponseModel> CheckCompanySubscription(CheckCompanySubscriptionModel model);
    }
}
