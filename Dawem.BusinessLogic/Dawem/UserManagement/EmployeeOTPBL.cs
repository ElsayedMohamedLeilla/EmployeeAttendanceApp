using Dawem.Contract.BusinessLogic.Dawem.Employees;
using Dawem.Contract.BusinessLogic.Dawem.Provider;
using Dawem.Contract.BusinessValidation.Dawem.Employees;
using Dawem.Contract.Repository.Manager;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.UserManagement;
using Dawem.Helpers;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Dawem.Employees.Users;
using Dawem.Models.Dtos.Dawem.Shared;
using Dawem.Models.DTOs.Dawem.Generic.Exceptions;
using Dawem.Translations;
using Dawem.Validation.FluentValidation.Dawem.Employees.User;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Dawem.BusinessLogic.Dawem.UserManagement
{
    public class EmployeeOTPBL : IEmployeeOTPBL
    {
        private readonly IUnitOfWork<ApplicationDBContext> unitOfWork;
        private readonly RequestInfo requestInfo;
        private readonly IEmployeeOTPBLValidation EmployeeOTPBLValidation;
        private readonly IRepositoryManager repositoryManager;
        private readonly IMailBL mailBL;
        public EmployeeOTPBL(IUnitOfWork<ApplicationDBContext> _unitOfWork,
             IRepositoryManager _repositoryManager,
             IMailBL _mailBL,
             RequestInfo _requestHeaderContext,
             IEmployeeOTPBLValidation _EmployeeOTPBLValidation)
        {
            unitOfWork = _unitOfWork;
            requestInfo = _requestHeaderContext;
            repositoryManager = _repositoryManager;
            EmployeeOTPBLValidation = _EmployeeOTPBLValidation;
            mailBL = _mailBL;
        }


        public async Task<string> PreSignUp(PreSignUpDTO model)
        {
            #region Model Validation
            var PreSignUpModel = new PreSignUpModelValidator();
            var createUserModelResult = PreSignUpModel.Validate(model);
            if (!createUserModelResult.IsValid)
            {
                var error = createUserModelResult.Errors.FirstOrDefault();
                throw new BusinessValidationException(error.ErrorMessage);
            }
            #endregion

            #region Validate Employee Signed Before
            var getCompany = await repositoryManager.CompanyRepository.GetEntityByConditionWithTrackingAsync(c => !c.IsDeleted
            && c.IdentityCode == model.CompanyVerificationCode && c.IsActive);

            if (getCompany == null) //company verification Code  not found
            {
                throw new BusinessValidationException(AmgadKeys.SorryCompanyVerificationCodeNotFound);
            }
            var getEmployee = await repositoryManager.EmployeeRepository.GetEntityByConditionWithTrackingAsync(e => !e.IsDeleted
            && e.EmployeeNumber == model.EmployeeNumber && e.CompanyId == getCompany.Id);
            if (getEmployee == null) // if employee Number not belong to any employee
            {
                throw new BusinessValidationException(AmgadKeys.SorryEmployeeNumberNotFound);
            }
            var getUser = await repositoryManager.UserRepository.GetEntityByConditionWithTrackingAsync(user => !user.IsDeleted
               && user.EmployeeId == getEmployee.Id && user.CompanyId == getCompany.Id);
            if (getUser != null) // this employee number already connected to user
            {
                throw new BusinessValidationException(AmgadKeys.SorryThisUserAlreadySignedUp);
            }
            var getUserByUserName = await repositoryManager.UserRepository.GetEntityByConditionWithTrackingAsync(user => !user.IsDeleted
              && user.UserName == getEmployee.Email && user.CompanyId == getCompany.Id);
            if (getUserByUserName != null) // this employee Name already Signed Up
            {
                throw new BusinessValidationException(AmgadKeys.SorryThisUserNameAlreadySignedUp);
            }
            var getUserByUserEmail = await repositoryManager.UserRepository.GetEntityByConditionWithTrackingAsync(user => !user.IsDeleted
             && user.Email == getEmployee.Email && user.CompanyId == getCompany.Id);
            if (getUserByUserEmail != null) // this employee Email already Signed Up
            {
                throw new BusinessValidationException(AmgadKeys.SorryThisEmailAlreadySignedUp);
            }
            #endregion

            #region CreateOTP Save DB
            #region GetMaxCode
            var getNextCode = await repositoryManager.EmployeeOTPRepository
                  .Get(e => e.EmployeeId == getEmployee.Id && e.CompanyId == getCompany.Id
                   && !e.IsDeleted && e.IsActive)
                  .Select(e => e.OTPCount)
                  .DefaultIfEmpty()
                  .MaxAsync() + 1;
            if (getNextCode > 5)
            {
                throw new BusinessValidationException(AmgadKeys.SorryYouHaveExceededTheNumberOfAttemptsPleaseContactTheAdministratorSoYouCanTryAgain);
            }
            #endregion
            unitOfWork.CreateTransaction();
            EmployeeOTP savedOtp = new();
            int savedOTP = GenerateRandomOTP();
            savedOtp.AddedApplicationType = requestInfo.ApplicationType;
            savedOtp.AddedDate = DateTime.UtcNow;
            savedOtp.OTPCount = getNextCode;
            savedOtp.IsVerified = false;
            savedOtp.IsActive = true;
            savedOtp.IsDeleted = false;
            savedOtp.ExpirationTime = savedOtp.AddedDate.AddMinutes(30); //expire after half hour
            savedOtp.CompanyId = getCompany.Id;
            savedOtp.OTP = savedOTP;
            savedOtp.EmployeeId = getEmployee.Id;
            repositoryManager.EmployeeOTPRepository.Insert(savedOtp);
            await unitOfWork.SaveAsync();
            #endregion


            await unitOfWork.CommitAsync();
            #region Send Email With OTP
            if (savedOtp.Id > 0) //save is success
            {

                var verifyEmail = new VerifyEmailModel
                {
                    Email = getEmployee.Email,
                    Subject = TranslationHelper.GetTranslation(AmgadKeys.ThanksForUsingDawemApplication, requestInfo?.Lang),
                    Body = $@"
        <!DOCTYPE html>
        <html>
        <head>
            <title>OTP Email</title>
        </head>
        <body>
            <p>
                {TranslationHelper.GetTranslation(AmgadKeys.PleaseUseThisOneTimePasswordToCompleateSignUpProcessThisOTPWillExpireAfterHalfHourFromNow, requestInfo?.Lang)}
            </p>
            <p>
                <strong>  {LeillaKeys.Space + savedOTP} </strong> 
            </p>
            <p>
                
                    {TranslationHelper.GetTranslation(AmgadKeys.PleaseManuallyCopyTheOneTimePasswordToCompleteTheSignUpProcess, requestInfo?.Lang)}
            </p>
        </body>
        </html>
                    "
                };

                await mailBL.SendEmail(verifyEmail);
            }
            #endregion
            #region Handle Response
            return getEmployee.Email;
            #endregion

        }
        public async Task<bool> Delete(int userId)
        {
            MyUser user = await repositoryManager.UserRepository.
                GetEntityByConditionWithTrackingAsync(user => !user.IsDeleted && user.Id == userId &&
                user.Type == requestInfo.Type &&
                ((requestInfo.CompanyId > 0 && user.CompanyId == requestInfo.CompanyId) ||
                (requestInfo.CompanyId <= 0 && user.CompanyId == null))) ??
                throw new BusinessValidationException(LeillaKeys.SorryUserNotFound);
            user.Delete();
            await unitOfWork.SaveAsync();
            return true;
        }
        public Task<bool> SendOTPByEmail(SendOTPByEmailDTO model)
        {
            throw new NotImplementedException();
        }
        public Task<string> GetOTPByEmployeeId(int employeeId)
        {
            throw new NotImplementedException();
        }

        public static int GenerateRandomOTP()
        {
            Random random = new Random();
            int otp = 0;
            for (int i = 0; i < 5; i++)
            {
                otp = otp * 10 + random.Next(0, 10);
            }
            return otp;
        }

        public async Task<bool> DeleteOTPsByEmployeeNumber(int employeeId)
        {
            if(employeeId == 0)
            {
                throw new BusinessValidationException(LeillaKeys.SorryYouMustEnterEmployeeId);
            }

            #region get Employee
            var getEmployee = repositoryManager.EmployeeRepository.Get(e => !e.IsDeleted && e.Id == employeeId).FirstOrDefault();
            #endregion

            var employeeOTPs = await repositoryManager.EmployeeOTPRepository.Get(o => !o.IsDeleted && o.IsActive && o.EmployeeId == getEmployee.Id).ToListAsync();
            repositoryManager.EmployeeOTPRepository.BulkDeleteIfExist(employeeOTPs);
            return true;

        }
    }
}

