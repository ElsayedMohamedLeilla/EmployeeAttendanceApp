using Dawem.Contract.BusinessValidation.Dawem.Employees;
using Dawem.Contract.Repository.Manager;
using Dawem.Enums.Generals;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Dawem.Employees.Users;
using Dawem.Models.DTOs.Dawem.Generic.Exceptions;
using Dawem.Translations;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Dawem.Validation.BusinessValidation.Dawem.Employees
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
        public async Task<int> SignUpValidation(UserSignUpModel model)
        {
            #region Validate Email

            //var checkEmailDuplicate = await repositoryManager.UserRepository
            //.Get(u => u.CompanyId == model.CompanyId &&
            //u.Email == model.Email.Trim()).AnyAsync();

            //if (checkEmailDuplicate)
            //{
            //    throw new BusinessValidationException(LeillaKeys.SorryUserEmailIsDuplicatedYouMustEnterUniqueEmail);
            //}

            #endregion

            #region Validate Mobile Number

            //var checkMobileDuplicate = await repositoryManager.UserRepository
            //.Get(u => u.CompanyId == model.CompanyId &&
            //u.MobileNumber == model.MobileNumber.Trim())
            //.AnyAsync();

            //if (checkMobileDuplicate)
            //{
            //    throw new BusinessValidationException(LeillaKeys.SorryUserMobileNumberIsDuplicatedYouMustEnterUniqueMobileNumber);
            //}

            #endregion

            #region Employee Number Validation

            //int? getEmployeeId = await repositoryManager.EmployeeRepository
            //    .Get(e => !e.IsDeleted && e.CompanyId == model.CompanyId && e.EmployeeNumber == model.EmployeeNumber).AnyAsync() ?
            //    await repositoryManager.EmployeeRepository
            //    .Get(e => !e.IsDeleted && e.CompanyId == model.CompanyId && e.EmployeeNumber == model.EmployeeNumber)
            //    .Select(e => e.Id)
            //    .FirstOrDefaultAsync() : null;

            //if (getEmployeeId == null)
            //{
            //    throw new BusinessValidationException(LeillaKeys.SorryCannotFindEmployeeWithEnteredEmployeeNumber);
            //}

            #endregion



            //return getEmployeeId ?? 0;
            return 0;
        }
        public async Task<bool> VerifyEmailValidation(UserVerifyEmailModel model)
        {
            var getUser = await repositoryManager.UserRepository.Get(c => c.Id == model.UserId)
                .Select(u => new
                {
                    u.VerificationCode,
                    u.EmailConfirmed,
                    u.VerificationCodeSendDate
                }).FirstOrDefaultAsync();

            if (getUser == null)
            {
                throw new BusinessValidationException(LeillaKeys.SorryUserNotFound);
            }

            #region Validate Verification Code

            if (getUser.EmailConfirmed)
            {
                throw new BusinessValidationException(LeillaKeys.SorryEmailAlreadyConfirmed);
            }

            if (getUser.VerificationCode != model.VerificationCode)
            {
                throw new BusinessValidationException(LeillaKeys.SorryEnteredVerificationCodeIsNotCorrect);
            }

            var getLifeTime = (DateTime.UtcNow - getUser.VerificationCodeSendDate).TotalMinutes;
            if (getLifeTime > 30)
            {
                throw new BusinessValidationException(LeillaKeys.SorryEnteredVerificationCodeIsExpiredYouMustSendAnotherOne);
            }


            #endregion

            return true;
        }
        public async Task<bool> SendVerificationCodeValidation(SendVerificationCodeModel model)
        {
            var getUser = await repositoryManager
                .UserRepository.Get(c => c.Id == model.UserId)
                .Select(u => new
                {
                    u.EmailConfirmed,
                    u.VerificationCodeSendDate
                }).FirstOrDefaultAsync();

            if (getUser is null)
            {
                throw new BusinessValidationException(LeillaKeys.SorryUserNotFound);
            }

            #region Send Validate Verification Code

            if (getUser.EmailConfirmed)
            {
                throw new BusinessValidationException(LeillaKeys.SorryEmailAlreadyConfirmed);
            }

            var getLifeTime = (DateTime.UtcNow - getUser.VerificationCodeSendDate).TotalSeconds;
            if (getLifeTime < 60)
            {
                throw new BusinessValidationException(LeillaKeys.SorryYouCanNotSendAnotherCodeUnlessOneMinutePassed);
            }

            #endregion

            return true;
        }
        public async Task<bool> CreateValidation(CreateUserModel model)
        {
            //var checkUserDuplicate = await repositoryManager
            //    .UserRepository.Get(c => !c.IsDeleted && c.CompanyId == requestInfo.CompanyId &&
            //    c.Name == model.Name).AnyAsync();
            //if (checkUserDuplicate)
            //{
            //    throw new BusinessValidationException(LeillaKeys.SorryUserNameIsDuplicated);
            //}

            //#region Validate Email

            //var checkEmailDuplicate = await repositoryManager.UserRepository
            //.Get(u => u.Email == model.Email.Trim()).AnyAsync();

            //if (checkEmailDuplicate)
            //{
            //    throw new BusinessValidationException(LeillaKeys.SorryUserEmailIsDuplicatedYouMustEnterUniqueEmail);
            //}

          //  #endregion

            //#region Validate Mobile Number

            //var checkMobileDuplicate = await repositoryManager.UserRepository
            //.Get(u => u.MobileNumber == model.MobileNumber.Trim())
            //.AnyAsync();

            //if (checkMobileDuplicate)
            //{
            //    throw new BusinessValidationException(LeillaKeys.SorryUserMobileNumberIsDuplicatedYouMustEnterUniqueMobileNumber);
            //}

            //#endregion

            #region Validate Employee

            if(model.EmployeeId == 0)
            {
                throw new BusinessValidationException(AmgadKeys.SorryYouMustChooseEmployeeToThisUser);
            }
            if (model.EmployeeId > 0)
            {
                var checkEmployee = await repositoryManager.EmployeeRepository
                                    .Get(e => e.Id == model.EmployeeId && !e.IsDeleted)
                                    .AnyAsync();

                if (!checkEmployee)
                {
                    throw new BusinessValidationException(LeillaKeys.SorrySelectedEmployeeNotFound);
                }
                var CheckUser = await repositoryManager.UserRepository
                                    .Get(e => e.EmployeeId == model.EmployeeId && !e.IsDeleted)
                                    .AnyAsync();
                if(CheckUser)
                {
                    throw new BusinessValidationException(AmgadKeys.SorrySelectedEmployeeAlreadyHaveUserPleaseSelectOtherEmployee);
                }

            }

            #endregion

            #region Validate Responsibilities

            var checkResponsibilitiesIds = await repositoryManager.ResponsibilityRepository.
                Get(responsibility => responsibility.CompanyId == requestInfo.CompanyId && responsibility.Type == AuthenticationType.DawemAdmin &&
                model.Responsibilities.Contains(responsibility.Id)).
                Select(responsibility => responsibility.Id).
                ToListAsync();

            if (checkResponsibilitiesIds.Count != model.Responsibilities.Count)
            {
                throw new BusinessValidationException(LeillaKeys.SorrySomeOrAllResponsibilitiesNotFound);
            }

            #endregion

            return true;
        }
        public async Task<bool> UpdateValidation(UpdateUserModel model)
        {
            //var checkUserDuplicate = await repositoryManager
            //    .UserRepository.Get(c => !c.IsDeleted && c.CompanyId == requestInfo.CompanyId &&
            //    c.Name == model.Name && c.Id != model.Id).AnyAsync();
            //if (checkUserDuplicate)
            //{
            //    throw new BusinessValidationException(LeillaKeys.SorryUserNameIsDuplicated);
            //}

            //#region Validate Email

            //var checkEmailDuplicate = await repositoryManager.UserRepository
            //.Get(u => u.Id != model.Id && u.Email == model.Email.Trim()).AnyAsync();

            //if (checkEmailDuplicate)
            //{
            //    throw new BusinessValidationException(LeillaKeys.SorryUserEmailIsDuplicatedYouMustEnterUniqueEmail);
            //}

            //#endregion

            //#region Validate Mobile Number

            //var checkMobileDuplicate = await repositoryManager.UserRepository
            //.Get(u => u.Id != model.Id && u.MobileNumber == model.MobileNumber.Trim())
            //.AnyAsync();

            //if (checkMobileDuplicate)
            //{
            //    throw new BusinessValidationException(LeillaKeys.SorryUserMobileNumberIsDuplicatedYouMustEnterUniqueMobileNumber);
            //}

            //#endregion

            #region Validate Employee
            if (model.EmployeeId == 0)
            {
                throw new BusinessValidationException(AmgadKeys.SorryYouMustChooseEmployeeToThisUser);
            }
            if (model.EmployeeId > 0)
            {
                var checkEmployee = await repositoryManager.EmployeeRepository
                                    .Get(e => e.Id == model.EmployeeId && !e.IsDeleted)
                                    .AnyAsync();

                if (!checkEmployee)
                {
                    throw new BusinessValidationException(LeillaKeys.SorrySelectedEmployeeNotFound);
                }
                var checkUser = await repositoryManager.UserRepository
                                   .Get(e => e.Id != model.Id && e.EmployeeId == model.EmployeeId  && !e.IsDeleted)
                                   .AnyAsync();

                if (checkUser)
                {
                    throw new BusinessValidationException(AmgadKeys.SorrySelectedEmployeeAlreadyHaveUserPleaseSelectOtherEmployee);
                }

            }

            #endregion

            #region Validate Responsibilities

            var checkResponsibilitiesIds = await repositoryManager.ResponsibilityRepository.
                Get(responsibility => responsibility.CompanyId == requestInfo.CompanyId && responsibility.Type == AuthenticationType.DawemAdmin &&
                model.Responsibilities.Contains(responsibility.Id)).
                Select(responsibility => responsibility.Id).
                ToListAsync();

            if (checkResponsibilitiesIds.Count != model.Responsibilities.Count)
            {
                throw new BusinessValidationException(LeillaKeys.SorrySomeOrAllResponsibilitiesNotFound);
            }

            #endregion

            return true;
        }
        public async Task<bool> AdminPanelCreateValidation(AdminPanelCreateUserModel model)
        {
            var checkUserDuplicate = await repositoryManager
                .UserRepository.Get(c => !c.IsDeleted && c.Type == AuthenticationType.AdminPanel && c.CompanyId == null &&
                c.Name == model.Name).AnyAsync();
            if (checkUserDuplicate)
            {
                throw new BusinessValidationException(LeillaKeys.SorryUserNameIsDuplicated);
            }

            #region Validate Email

            var checkEmailDuplicate = await repositoryManager.UserRepository
            .Get(u => u.Email == model.Email.Trim()).AnyAsync();

            if (checkEmailDuplicate)
            {
                throw new BusinessValidationException(LeillaKeys.SorryUserEmailIsDuplicatedYouMustEnterUniqueEmail);
            }

            #endregion

            #region Validate Responsibilities

            var checkResponsibilitiesIds = await repositoryManager.ResponsibilityRepository.
                Get(responsibility => responsibility.CompanyId == null && responsibility.Type == AuthenticationType.AdminPanel &&
                model.Responsibilities.Contains(responsibility.Id)).
                Select(responsibility => responsibility.Id).
                ToListAsync();

            if (checkResponsibilitiesIds.Count != model.Responsibilities.Count)
            {
                throw new BusinessValidationException(LeillaKeys.SorrySomeOrAllResponsibilitiesNotFound);
            }

            #endregion

            return true;
        }
        public async Task<bool> AdminPanelUpdateValidation(AdminPanelUpdateUserModel model)
        {
            var checkUserDuplicate = await repositoryManager
                .UserRepository.Get(c => !c.IsDeleted && c.Type == AuthenticationType.AdminPanel && c.CompanyId == null &&
                c.Name == model.Name && c.Id != model.Id).AnyAsync();
            if (checkUserDuplicate)
            {
                throw new BusinessValidationException(LeillaKeys.SorryUserNameIsDuplicated);
            }

            #region Validate Email

            var checkEmailDuplicate = await repositoryManager.UserRepository
            .Get(u => u.Id != model.Id && u.Email == model.Email.Trim()).AnyAsync();

            if (checkEmailDuplicate)
            {
                throw new BusinessValidationException(LeillaKeys.SorryUserEmailIsDuplicatedYouMustEnterUniqueEmail);
            }

            #endregion

            #region Validate Responsibilities

            var checkResponsibilitiesIds = await repositoryManager.ResponsibilityRepository.
                Get(responsibility => responsibility.CompanyId == null && responsibility.Type == AuthenticationType.AdminPanel &&
                model.Responsibilities.Contains(responsibility.Id)).
                Select(responsibility => responsibility.Id).
                ToListAsync();

            if (checkResponsibilitiesIds.Count != model.Responsibilities.Count)
            {
                throw new BusinessValidationException(LeillaKeys.SorrySomeOrAllResponsibilitiesNotFound);
            }

            #endregion

            return true;
        }
    }
}
