using Dawem.Contract.BusinessValidation.Employees;
using Dawem.Contract.Repository.Manager;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Employees.User;
using Dawem.Models.Exceptions;
using Dawem.Translations;
using Microsoft.EntityFrameworkCore;

namespace Dawem.Validation.BusinessValidation.Users
{

    public class UserBLValidation : IUserBLValidation
    {
        private readonly IRepositoryManager repositoryManager;
        private readonly RequestInfo requestInfo;
        public UserBLValidation(IRepositoryManager _repositoryManager, RequestInfo _requestInfo)
        {
            repositoryManager = _repositoryManager;
            requestInfo = _requestInfo;
        }
        public async Task<bool> CreateValidation(CreateUserModel model)
        {
            var checkUserDuplicate = await repositoryManager
                .UserRepository.Get(c => c.CompanyId == requestInfo.CompanyId &&
                c.Name == model.Name).AnyAsync();
            if (checkUserDuplicate)
            {
                throw new BusinessValidationException(DawemKeys.SorryUserNameIsDuplicated);
            }

            #region Validate Email

            var checkEmailDuplicate = await repositoryManager.UserRepository
            .Get(u => u.Email == model.Email.Trim()).AnyAsync();

            if (checkEmailDuplicate)
            {
                throw new BusinessValidationException(DawemKeys.SorryUserEmailIsDuplicatedYouMustEnterUniqueEmail);
            }

            #endregion

            #region Validate Mobile Number

            var checkMobileDuplicate = await repositoryManager.UserRepository
            .Get(u =>  u.MobileNumber == model.MobileNumber.Trim())
            .AnyAsync();

            if (checkMobileDuplicate)
            {
                throw new BusinessValidationException(DawemKeys.SorryUserMobileNumberIsDuplicatedYouMustEnterUniqueMobileNumber);
            }

            #endregion

            #region Validate Employee

            if (model.EmployeeId > 0)
            {
                var checkEmployee = await repositoryManager.EmployeeRepository
                                    .Get(e => e.Id == model.EmployeeId && !e.IsDeleted)
                                    .AnyAsync();

                if (!checkEmployee)
                {
                    throw new BusinessValidationException(DawemKeys.SorrySelectedEmployeeNotFound);
                }
            }

            #endregion

            return true;
        }
        public async Task<bool> UpdateValidation(UpdateUserModel model)
        {
            var checkUserDuplicate = await repositoryManager
                .UserRepository.Get(c => c.CompanyId == requestInfo.CompanyId &&
                c.Name == model.Name && c.Id != model.Id).AnyAsync();
            if (checkUserDuplicate)
            {
                throw new BusinessValidationException(DawemKeys.SorryUserNameIsDuplicated);
            }

            #region Validate Email

            var checkEmailDuplicate = await repositoryManager.UserRepository
            .Get(u => u.Id != model.Id && u.Email == model.Email.Trim()).AnyAsync();

            if (checkEmailDuplicate)
            {
                throw new BusinessValidationException(DawemKeys.SorryUserEmailIsDuplicatedYouMustEnterUniqueEmail);
            }

            #endregion

            #region Validate Mobile Number

            var checkMobileDuplicate = await repositoryManager.UserRepository
            .Get(u => u.Id != model.Id && u.MobileNumber == model.MobileNumber.Trim())
            .AnyAsync();

            if (checkMobileDuplicate)
            {
                throw new BusinessValidationException(DawemKeys.SorryUserMobileNumberIsDuplicatedYouMustEnterUniqueMobileNumber);
            }

            #endregion

            #region Validate Employee

            if (model.EmployeeId > 0)
            {
                var checkEmployee = await repositoryManager.EmployeeRepository
                                    .Get(e => e.Id == model.EmployeeId && !e.IsDeleted)
                                    .AnyAsync();

                if (!checkEmployee)
                {
                    throw new BusinessValidationException(DawemKeys.SorrySelectedEmployeeNotFound);
                }
            }

            #endregion

            return true;
        }
    }
}
