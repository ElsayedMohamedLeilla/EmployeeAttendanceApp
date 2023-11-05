using Dawem.Contract.BusinessValidation;
using Dawem.Contract.Repository.Manager;
using Dawem.Domain.Entities.UserManagement;
using Dawem.Models.Dtos.Identity;
using Dawem.Models.Dtos.Provider;
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
        public AccountBLValidation(IRepositoryManager _repositoryManager, UserManagerRepository _userManagerRepository)
        {
            repositoryManager = _repositoryManager;
            userManagerRepository = _userManagerRepository;
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
            #region Find User

            var user = await userManagerRepository.FindByNameAsync(model.Email) ?? 
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
                throw new BusinessValidationException(LeillaKeys.SorryEmailNotConfirmedPleaseCheckYourEmail);
            }    
            return user;
        }
    }
}
