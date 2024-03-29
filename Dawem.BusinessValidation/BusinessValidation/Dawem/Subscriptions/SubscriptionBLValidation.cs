using Dawem.Contract.BusinessValidation.Dawem.Subscriptions;
using Dawem.Contract.Repository.Manager;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Dawem.Subscriptions;
using Dawem.Models.Generic.Exceptions;
using Dawem.Translations;
using Microsoft.EntityFrameworkCore;


namespace Dawem.Validation.BusinessValidation.Dawem.Subscriptions
{

    public class SubscriptionBLValidation : ISubscriptionBLValidation
    {
        private readonly IRepositoryManager repositoryManager;
        private readonly RequestInfo requestInfo;
        public SubscriptionBLValidation(IRepositoryManager _repositoryManager, RequestInfo _requestInfo)
        {
            repositoryManager = _repositoryManager;
            requestInfo = _requestInfo;
        }
        public async Task<bool> CreateValidation(CreateSubscriptionModel model)
        {
            var checkSubscriptionDuplicate = await repositoryManager
                .SubscriptionRepository.Get(c => !c.IsDeleted && c.CompanyId == model.CompanyId)
                .AnyAsync();
            if (checkSubscriptionDuplicate)
            {
                throw new BusinessValidationException(LeillaKeys.SorryThereIsSubscriptionAlreadyWithThisCompany);
            }

            return true;
        }
        public async Task<bool> UpdateValidation(UpdateSubscriptionModel model)
        {
            var checkSubscriptionDuplicate = await repositoryManager
                .SubscriptionRepository.Get(c => !c.IsDeleted && c.CompanyId == model.CompanyId && c.Id != model.Id)
                .AnyAsync();
            if (checkSubscriptionDuplicate)
            {
                throw new BusinessValidationException(LeillaKeys.SorryThereIsSubscriptionAlreadyWithThisCompany);
            }

            return true;
        }
    }
}
