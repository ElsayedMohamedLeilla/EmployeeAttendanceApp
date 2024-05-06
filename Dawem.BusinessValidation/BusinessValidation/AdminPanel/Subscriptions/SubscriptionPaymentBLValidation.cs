using Dawem.Contract.BusinessValidation.AdminPanel.Subscriptions;
using Dawem.Contract.Repository.Manager;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Dawem.Subscriptions.SubscriptionPayment;


namespace Dawem.Validation.BusinessValidation.AdminPanel.Subscriptions
{

    public class SubscriptionPaymentBLValidation : ISubscriptionPaymentBLValidation
    {
        private readonly IRepositoryManager repositoryManager;
        private readonly RequestInfo requestInfo;
        public SubscriptionPaymentBLValidation(IRepositoryManager _repositoryManager,
            RequestInfo _requestInfo)
        {
            repositoryManager = _repositoryManager;
            requestInfo = _requestInfo;
        }
        public async Task<bool> CreateValidation(CreateSubscriptionPaymentModel model)
        {
            return true;
        }
        public async Task<bool> UpdateValidation(UpdateSubscriptionPaymentModel model)
        {
            return true;
        }
    }
}
