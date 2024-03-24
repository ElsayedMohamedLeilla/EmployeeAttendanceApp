using Dawem.Models.Dtos.Subscriptions;

namespace Dawem.Contract.BusinessValidation.Subscriptions
{
    public interface ISubscriptionBLValidation
    {
        Task<bool> CreateValidation(CreateSubscriptionModel model);
        Task<bool> UpdateValidation(UpdateSubscriptionModel model);
    }
}
