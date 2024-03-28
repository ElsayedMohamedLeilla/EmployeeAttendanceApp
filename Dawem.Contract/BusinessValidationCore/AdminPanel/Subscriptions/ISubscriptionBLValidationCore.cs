using Dawem.Models.Criteria.Subscriptions;
using Dawem.Models.Response.Dawem.Subscriptions;

namespace Dawem.Contract.BusinessValidationCore.AdminPanel.Subscriptions
{
    public interface ISubscriptionBLValidationCore
    {
        Task<CheckCompanySubscriptionResponseModel> CheckCompanySubscription(CheckCompanySubscriptionModel model);
    }
}
