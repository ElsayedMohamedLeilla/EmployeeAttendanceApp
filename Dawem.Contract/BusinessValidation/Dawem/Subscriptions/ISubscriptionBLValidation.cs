using Dawem.Models.Dtos.Dawem.Subscriptions;

namespace Dawem.Contract.BusinessValidation.Dawem.Subscriptions
{
    public interface ISubscriptionBLValidation
    {
        Task<bool> CreateValidation(CreateSubscriptionModel model);
        Task<bool> UpdateValidation(UpdateSubscriptionModel model);
    }
}
