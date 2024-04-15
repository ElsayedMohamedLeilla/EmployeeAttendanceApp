using Dawem.Models.Criteria.Subscriptions;
using Dawem.Models.Response.AdminPanel.Subscriptions;

namespace Dawem.Contract.BusinessValidationCore.AdminPanel.Subscriptions
{
    public interface ISubscriptionBLValidationCore
    {
        Task<CheckCompanySubscriptionResponseModel> CheckCompanySubscription(CheckCompanySubscriptionModel model);
    }
}
