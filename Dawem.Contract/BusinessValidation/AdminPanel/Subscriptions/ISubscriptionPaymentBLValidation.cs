using Dawem.Models.Dtos.Dawem.Subscriptions.Plans;

namespace Dawem.Contract.BusinessValidation.AdminPanel.Subscriptions
{
    public interface ISubscriptionPaymentBLValidation
    {
        Task<bool> CreateValidation(CreateSubscriptionPaymentModel model);
        Task<bool> UpdateValidation(UpdateSubscriptionPaymentModel model);
    }
}
