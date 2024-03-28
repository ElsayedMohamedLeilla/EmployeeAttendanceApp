using Dawem.Contract.BusinessValidation;
using Dawem.Contract.BusinessValidationCore.AdminPanel.Subscriptions;
using Dawem.Contract.Repository.Manager;
using Dawem.Domain.Entities.UserManagement;
using Dawem.Enums.Generals;
using Dawem.Models.Criteria.Subscriptions;
using Dawem.Models.Dtos.Dawem.Identities;
using Dawem.Models.Dtos.Dawem.Providers;
using Dawem.Models.Exceptions;
using Dawem.Repository.UserManagement;
using Dawem.Translations;
using Microsoft.EntityFrameworkCore;

namespace Dawem.Validation.BusinessValidation
{

    public class AccountBLValidation : IAccountBLValidation
    {
        private readonly IRepositoryManager repositoryManager;
        private readonly UserManagerRepository userManagerRepository;
        private readonly ISubscriptionBLValidationCore subscriptionBLValidationCore;
        public AccountBLValidation(IRepositoryManager _repositoryManager, UserManagerRepository _userManagerRepository,
            ISubscriptionBLValidationCore _subscriptionBLValidationCore)
        {
            repositoryManager = _repositoryManager;
            userManagerRepository = _userManagerRepository;
            subscriptionBLValidationCore = _subscriptionBLValidationCore;
        }
        public async Task<bool> SignUpValidation(SignUpModel model)
        {
            var checkCompanyemailDuplicate = await repositoryManager
                .CompanyRepository.Get(c => c.Email == model.CompanyEmail).AnyAsync();
            if (checkCompanyemailDuplicate)
            {
                throw new BusinessValidationException(LeillaKeys.SorryCompanyEmailAlreadyUsedPleaseSelectAnotherOne);
            }

            var checkCompanyNameDuplicate = await repositoryManager
                .CompanyRepository.Get(c => c.Name == model.CompanyName).AnyAsync();
            if (checkCompanyNameDuplicate)
            {
                throw new BusinessValidationException(LeillaKeys.SorryCompanyNameAlreadyUsedPleaseSelectAnotherOne);
            }

            var checkUserEmailDuplicate = await repositoryManager
                .UserRepository.Get(c => c.Email == model.UserEmail).AnyAsync();
            if (checkCompanyemailDuplicate)
            {
                throw new BusinessValidationException(LeillaKeys.SorryUserEmailAlreadyUsedPleaseSelectAnotherOne);
            }

            return true;
        }
        public async Task<MyUser> SignInValidation(SignInModel model)
        {
            #region Check Company code

            if (model.CompanyId > 0)
            {
                var checkCompanyCode = await repositoryManager.CompanyRepository
               .Get(c => !c.IsDeleted && c.Id == model.CompanyId)
               .AnyAsync();

                if (!checkCompanyCode)
                {
                    throw new BusinessValidationException(LeillaKeys.SorryCannotFindTheCompany);
                }
            }

            #endregion

            #region Check Company Subscription

            var checkCompanySubscriptionModel = new CheckCompanySubscriptionModel
            {
                CompanyId = model.CompanyId,
                FromType = CheckCompanySubscriptionFromType.LogIn
            };
            await subscriptionBLValidationCore.CheckCompanySubscription(checkCompanySubscriptionModel);

            #endregion

            #region Try Find User

            var user = await repositoryManager.UserRepository
                .GetEntityByConditionAsync(u => !u.IsDeleted && u.Email == model.Email && (model.CompanyId <= 0 ||
                (u.Company != null && u.Company.Id == model.CompanyId))) ??
                throw new BusinessValidationException(LeillaKeys.SorryUserNotFound);

            #endregion

            #region Check Password

            bool checkPasswordAsyncRes = await userManagerRepository.CheckPasswordAsync(user, model.Password);
            if (!checkPasswordAsyncRes)
            {
                throw new BusinessValidationException(LeillaKeys.SorryPasswordIncorrectEnterCorrectPasswordForSelectedUser);
            }

            #endregion

            if (!user.EmailConfirmed)
            {
                throw new BusinessValidationExceptionGenaric<int>(LeillaKeys.SorryEmailNotConfirmedPleaseCheckYourEmail) { Data = user.Id };
            }

            return user;
        }
    }
}
