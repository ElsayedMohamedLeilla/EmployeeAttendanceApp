using AutoMapper;
using Dawem.Contract.BusinessLogic.Dawem.Employees;
using Dawem.Contract.BusinessLogic.Dawem.Provider;
using Dawem.Contract.BusinessLogicCore.Dawem;
using Dawem.Contract.BusinessValidation.Dawem.Employees;
using Dawem.Contract.Repository.Manager;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Core;
using Dawem.Domain.Entities.UserManagement;
using Dawem.Enums.Generals;
using Dawem.Helpers;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Dawem.Employees.Users;
using Dawem.Models.Dtos.Dawem.Shared;
using Dawem.Models.DTOs.Dawem.Generic.Exceptions;
using Dawem.Models.Response.Dawem.Employees.Users;
using Dawem.Repository.UserManagement;
using Dawem.Translations;
using Dawem.Validation.FluentValidation.Dawem.Employees.User;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Dawem.BusinessLogic.Dawem.UserManagement
{
    public class UserBL : IUserBL
    {
        private readonly IUnitOfWork<ApplicationDBContext> unitOfWork;
        private readonly RequestInfo requestInfo;
        private readonly IUserBLValidation userBLValidation;
        private readonly IRepositoryManager repositoryManager;
        private readonly IMapper mapper;
        private readonly IMailBL mailBL;
        private readonly IUploadBLC uploadBLC;
        private readonly UserManagerRepository userManagerRepository;
        public UserBL(IUnitOfWork<ApplicationDBContext> _unitOfWork,
            IRepositoryManager _repositoryManager,
            IMapper _mapper,
             IMailBL _mailBL,
            IUploadBLC _uploadBLC,
            UserManagerRepository _userManagerRepository,
           RequestInfo _requestHeaderContext,
           IUserBLValidation _userBLValidation)
        {
            unitOfWork = _unitOfWork;
            requestInfo = _requestHeaderContext;
            repositoryManager = _repositoryManager;
            userBLValidation = _userBLValidation;
            mapper = _mapper;
            mailBL = _mailBL;
            uploadBLC = _uploadBLC;
            userManagerRepository = _userManagerRepository;
        }


        public async Task<int> SignUp(UserSignUpModel model)
        {
            //#region business validation
            //var employeeid = await userBLValidation.SignUpValidation(model);
            //#endregion
            MyUser user = new();
            unitOfWork.CreateTransaction();

            #region get Company
            var getCompany = repositoryManager.CompanyRepository.Get(e => !e.IsDeleted && e.IsActive && e.IdentityCode == model.CompanyVerficationCode).FirstOrDefault();
            #endregion

            #region get Employee
            var getEmployee = repositoryManager.EmployeeRepository.Get(e => !e.IsDeleted && e.IsActive && e.EmployeeNumber == model.EmployeeNumber && e.CompanyId == getCompany.Id).FirstOrDefault();
            #endregion

            #region GetMax OTP
            var employeeOTPs = await repositoryManager.EmployeeOTPRepository.Get(o => !o.IsDeleted && o.IsActive && o.EmployeeId == getEmployee.Id).ToListAsync();
            if (employeeOTPs.Any())
            {
                // Retrieve the OTP with the maximum code
                var maxOTP = employeeOTPs.OrderByDescending(o => o.Code).First();
                // Check if the retrieved OTP has expired
                bool isExpired = maxOTP.ExpirationTime.AddHours(2) < DateTime.Now;
                if (isExpired)
                {
                    if (maxOTP.Code < 5)
                        throw new BusinessValidationException(AmgadKeys.SorryOTPIsExpiredPleaseTrySignUpAgain);
                    else
                        throw new BusinessValidationException(AmgadKeys.SorryOTPIsExpiredAndYouHaveExceededTheNumberOfAttemptsPleaseContactTheAdministratorSoYouCanTryAgain);
                }
                else
                {
                    #region Set User
                    var getNextCode = await repositoryManager.UserRepository
                        .Get(e => (requestInfo.Type == AuthenticationType.DawemAdmin && e.CompanyId == requestInfo.CompanyId) ||
                        (requestInfo.Type == AuthenticationType.AdminPanel && e.CompanyId == null))
                        .Select(e => e.Code)
                        .DefaultIfEmpty()
                        .MaxAsync() + 1;
                    #endregion

                    user.UserName = getEmployee.Email;
                    user.Email = getEmployee.Email;
                    user.MobileNumber = getEmployee.MobileNumber;
                    user.AddedDate = DateTime.UtcNow;
                    user.CompanyId = getCompany.Id;
                    user.MobileCountryId = getEmployee.MobileCountryId;
                    user.ProfileImageName = getEmployee.ProfileImageName;
                    user.Code = getNextCode;
                    user.EmployeeId = getEmployee.Id;
                    user.VerificationCode = maxOTP.OTP.ToString();
                    user.IsActive = true;
                    var createUserResponse = await userManagerRepository.CreateAsync(user, model.Password);

                    if (!createUserResponse.Succeeded)
                    {
                        unitOfWork.Rollback();
                        var error = createUserResponse.Errors.FirstOrDefault();
                        if (error.Code == "DuplicateUserName")
                            throw new BusinessValidationException(AmgadKeys.SorryThisUserNameAlreadySignedUp);
                        else if (error.Code == "DuplicateEmail")
                            throw new BusinessValidationException(AmgadKeys.SorryThisEmailAlreadySignedUp);
                        else
                            throw new BusinessValidationException(AmgadKeys.SorryErrorHappenWhileSigningUp);


                    }
                    else
                        repositoryManager.EmployeeOTPRepository.BulkDeleteIfExist(employeeOTPs);

                        #region Send Greetings Email

                    var greetingEmail = new VerifyEmailModel
                    {
                        Email = getEmployee.Email,
                        Subject = TranslationHelper.GetTranslation(LeillaKeys.ThanksForRegistrationOnDawem, requestInfo?.Lang),
                        Body = $"{getEmployee.Name},\n\n" +
                               $"{TranslationHelper.GetTranslation(AmgadKeys.WeAreThrilledToWelcomeYouToDawem, requestInfo?.Lang)},\n\n" +
                               $"{TranslationHelper.GetTranslation(AmgadKeys.BestRegards, requestInfo?.Lang)},\n\n"

                    };

                    await mailBL.SendEmail(greetingEmail);

                    #endregion

                }
            }
            else
            {
                throw new BusinessValidationException(AmgadKeys.SorryNoOTPWasFoundForThisUser);
            }
            #endregion
            #region Handle Response
            await unitOfWork.SaveAsync();
            await unitOfWork.CommitAsync();
            return user.Id;

            #endregion

        }
        public async Task<bool> VerifyEmail(UserVerifyEmailModel model)
        {
            #region Business Validation

            await userBLValidation.VerifyEmailValidation(model);

            #endregion

            unitOfWork.CreateTransaction();

            #region Verify Email

            var getUser = await repositoryManager.UserRepository.GetEntityByConditionWithTrackingAsync(user => !user.IsDeleted
              && user.Id == model.UserId && user.VerificationCode == model.VerificationCode);

            getUser.EmailConfirmed = true;

            var updateUserResponse = await userManagerRepository.UpdateAsync(getUser);

            if (!updateUserResponse.Succeeded)
            {
                await unitOfWork.RollbackAsync();
                throw new BusinessValidationException(LeillaKeys.SorryErrorHappenWhileUpdatingUser);
            }

            await unitOfWork.SaveAsync();

            #endregion

            #region Handle Response

            await unitOfWork.CommitAsync();
            return true;

            #endregion

        }
        public async Task<bool> SendVerificationCode(SendVerificationCodeModel model)
        {
            #region Business Validation

            await userBLValidation.SendVerificationCodeValidation(model);

            #endregion

            unitOfWork.CreateTransaction();

            #region Send Verification Code

            #region Get Verification Code

            var getUser = await repositoryManager.UserRepository.GetByIdAsync(model.UserId);
            string getNewVerificationCode = await GetVerificationCode(getUser.CompanyId ?? 0);

            #endregion


            getUser.VerificationCode = getNewVerificationCode;
            getUser.VerificationCodeSendDate = DateTime.UtcNow;

            var updateUserResponse = await userManagerRepository.UpdateAsync(getUser);

            if (!updateUserResponse.Succeeded)
            {
                await unitOfWork.RollbackAsync();
                throw new BusinessValidationException(LeillaKeys.SorryErrorHappenWhileSendVerificationCode);
            }

            #region Send New Verification Code In Email

            var verifyEmail = new VerifyEmailModel
            {
                Email = getUser.Email,
                Subject = TranslationHelper.GetTranslation(LeillaKeys.ThanksForRegistrationOnDawem, requestInfo?.Lang),
                Body = TranslationHelper.GetTranslation(LeillaKeys.YouAreDoneRegistrationSuccessfullyOnDawemYouMustEnterThisVerificationCodeOnDawemToVerifyYourEmailAndCanSignIn, requestInfo?.Lang)
                + getNewVerificationCode
            };

            await mailBL.SendEmail(verifyEmail);

            #endregion

            #endregion

            #region Handle Response

            await unitOfWork.CommitAsync();
            return true;

            #endregion

        }
        private async Task<string> GetVerificationCode(int? companyId)
        {
            var isVerificationCodeRepeated = false;
            var getNewVerificationCode = StringHelper.RandomNumber(6);
            do
            {
                isVerificationCodeRepeated = await repositoryManager.UserRepository
               .Get(e => e.CompanyId == companyId && e.VerificationCode == getNewVerificationCode)
               .AnyAsync();
            } while (isVerificationCodeRepeated);
            return getNewVerificationCode;
        }
        public async Task<int> Create(CreateUserModel model)
        {
            #region Model Validation

            var createUserModel = new CreateUserModelValidator();
            var createUserModelResult = createUserModel.Validate(model);
            if (!createUserModelResult.IsValid)
            {
                var error = createUserModelResult.Errors.FirstOrDefault();
                throw new BusinessValidationException(error.ErrorMessage);
            }

            #endregion

            #region Business Validation

            await userBLValidation.CreateValidation(model);

            #endregion

            unitOfWork.CreateTransaction();

            //#region Upload Profile Image

            //string imageName = null;
            //if (model.ProfileImageFile != null && model.ProfileImageFile.Length > 0)
            //{
            //    var result = await uploadBLC.UploadFile(model.ProfileImageFile, LeillaKeys.Users)
            //        ?? throw new BusinessValidationException(LeillaKeys.SorryErrorHappenWhileUploadProfileImage); ;
            //    imageName = result.FileName;
            //}

            //#endregion

            #region Insert User

            //#region Set User code

            //var getNextCode = await repositoryManager.UserRepository
            //    .Get(e => !e.IsDeleted && e.CompanyId == requestInfo.CompanyId)
            //    .Select(e => e.Code)
            //    .DefaultIfEmpty()
            //    .MaxAsync() + 1;

            //#endregion
            #region GetEmployee
            var foundEmployee = await repositoryManager.EmployeeRepository.Get(e =>
            !e.IsDeleted && e.CompanyId == requestInfo.CompanyId && e.Id == model.EmployeeId).FirstOrDefaultAsync();
            #endregion

            var user = mapper.Map<MyUser>(model);
            user.CompanyId = requestInfo.CompanyId;
            user.AddUserId = requestInfo.UserId;
            user.UserName = foundEmployee.Email + LeillaKeys.SpaceThenDashThenSpace + user.CompanyId;
            user.ProfileImageName = foundEmployee.ProfileImageName;
            user.Code = foundEmployee.Code;
            user.EmailConfirmed = true;
            user.PhoneNumberConfirmed = true;
            user.Type = AuthenticationType.DawemAdmin;

            var createUserResponse = await userManagerRepository.CreateAsync(user, model.Password);
            if (!createUserResponse.Succeeded)
            {
                unitOfWork.Rollback();
                throw new BusinessValidationException(LeillaKeys.SorryErrorHappenWhileAddingUser);
            }

            /*if (model.Responsibilities != null)
            {
                var getUserResponsibilities = await repositoryManager.ResponsibilityRepository
                    .Get(r => model.Responsibilities.Contains(r.Id))
                    .Select(r => r.Name)
                    .ToListAsync();

                if (getUserResponsibilities == null || getUserResponsibilities.Count == 0)
                    throw new BusinessValidationException(LeillaKeys.SorryYouMustEnterOneResponsibilityAtLeast);

                var assignRolesResult = await userManagerRepository.AddToRolesAsync(user, getUserResponsibilities);
                if (!assignRolesResult.Succeeded)
                {
                    unitOfWork.Rollback();
                    throw new BusinessValidationException(LeillaKeys.SorryErrorHappenWhileAddingUser);
                }
                await unitOfWork.SaveAsync();
            }*/

            #endregion

            #region Handle Response

            await unitOfWork.CommitAsync();
            return user.Id;

            #endregion

        }
        public async Task<int> AdminPanelCreate(AdminPanelCreateUserModel model)
        {
            #region Model Validation

            var createUserModel = new AdminPanelCreateUserModelValidator();
            var createUserModelResult = createUserModel.Validate(model);
            if (!createUserModelResult.IsValid)
            {
                var error = createUserModelResult.Errors.FirstOrDefault();
                throw new BusinessValidationException(error.ErrorMessage);
            }

            #endregion

            #region Business Validation

            await userBLValidation.AdminPanelCreateValidation(model);

            #endregion

            unitOfWork.CreateTransaction();

            #region Upload Profile Image

            string imageName = null;
            if (model.ProfileImageFile != null && model.ProfileImageFile.Length > 0)
            {
                var result = await uploadBLC.UploadFile(model.ProfileImageFile, LeillaKeys.Users)
                    ?? throw new BusinessValidationException(LeillaKeys.SorryErrorHappenWhileUploadProfileImage); ;
                imageName = result.FileName;
            }

            #endregion

            #region Insert User

            #region Set User code

            var getNextCode = await repositoryManager.UserRepository
                .Get(e => !e.IsDeleted && e.Type == AuthenticationType.AdminPanel && e.CompanyId == null)
                .Select(e => e.Code)
                .DefaultIfEmpty()
                .MaxAsync() + 1;

            #endregion

            var user = mapper.Map<MyUser>(model);
            user.AddUserId = requestInfo.UserId;
            user.UserName = model.Email + LeillaKeys.SpaceThenDashThenSpace + user.CompanyId;
            user.ProfileImageName = imageName;
            user.Code = getNextCode;
            user.EmailConfirmed = true;
            user.PhoneNumberConfirmed = true;
            user.Type = AuthenticationType.AdminPanel;

            var createUserResponse = await userManagerRepository.CreateAsync(user, model.Password);
            if (!createUserResponse.Succeeded)
            {
                unitOfWork.Rollback();
                throw new BusinessValidationException(LeillaKeys.SorryErrorHappenWhileAddingUser);
            }

            #endregion

            #region Handle Response

            await unitOfWork.CommitAsync();
            return user.Id;

            #endregion

        }
        public async Task<bool> Update(UpdateUserModel model)
        {

            #region Model Validation

            var updateUserModelValidator = new UpdateUserModelValidator();
            var updateUserModelValidatorResult = updateUserModelValidator.Validate(model);
            if (!updateUserModelValidatorResult.IsValid)
            {
                var error = updateUserModelValidatorResult.Errors.FirstOrDefault();
                throw new BusinessValidationException(error.ErrorMessage);
            }

            #endregion

            #region Business Validation

            await userBLValidation.UpdateValidation(model);

            #endregion

            unitOfWork.CreateTransaction();

            //#region Upload Profile Image

            //string imageName = null;
            //if (model.ProfileImageFile != null && model.ProfileImageFile.Length > 0)
            //{
            //    var result = await uploadBLC.UploadFile(model.ProfileImageFile, LeillaKeys.Users)
            //        ?? throw new BusinessValidationException(LeillaKeys.SorryErrorHappenWhileUploadProfileImage);
            //    imageName = result.FileName;
            //}

            //#endregion

            #region Update User

            var getUser = await repositoryManager.UserRepository.GetEntityByConditionWithTrackingAsync(user => !user.IsDeleted
            && user.Id == model.Id && user.CompanyId == requestInfo.CompanyId && user.Type == AuthenticationType.DawemAdmin);

            #region GetEmployee
            var foundEmployee = await repositoryManager.EmployeeRepository.Get(e =>
            !e.IsDeleted && e.CompanyId == requestInfo.CompanyId && e.Id == model.EmployeeId).FirstOrDefaultAsync();
            #endregion

            getUser.Name = foundEmployee.Name;
            getUser.EmployeeId = model.EmployeeId;
            getUser.Email = foundEmployee.Email;
            getUser.UserName = foundEmployee.Email;
            getUser.MobileNumber = foundEmployee.MobileNumber;
            getUser.IsActive = foundEmployee.IsActive;
            getUser.IsAdmin = model.IsAdmin;
            getUser.ModifiedDate = DateTime.Now;
            getUser.ModifyUserId = requestInfo.UserId;
            getUser.ProfileImageName = foundEmployee.ProfileImageName;

            var updateUserResponse = await userManagerRepository.UpdateAsync(getUser);

            if (!updateUserResponse.Succeeded)
            {
                await unitOfWork.RollbackAsync();
                throw new BusinessValidationException(LeillaKeys.SorryErrorHappenWhileUpdatingUser);
            }

            await unitOfWork.SaveAsync();

            #region Update Responsibilities

            var existDbList = repositoryManager.UserResponsibilityRepository
                    .GetByCondition(e => e.UserId == getUser.Id)
                    .ToList();

            var existingResponsibilityIds = existDbList.Select(e => e.ResponsibilityId).ToList();

            var addedUserResponsibilities = model.Responsibilities != null ? model.Responsibilities
                .Where(responsibilityId => !existingResponsibilityIds.Contains(responsibilityId))
                .Select(responsibilityId => new UserResponsibility
                {
                    UserId = model.Id,
                    ResponsibilityId = responsibilityId,
                    ModifyUserId = requestInfo.UserId,
                    ModifiedDate = DateTime.UtcNow
                }).ToList() : new List<UserResponsibility>();

            var responsibilitiesToRemove = existDbList
                .Where(ge => model.Responsibilities == null || !model.Responsibilities.Contains(ge.ResponsibilityId))
                .Select(ge => ge.ResponsibilityId)
                .ToList();

            var removedUserResponsibilities = repositoryManager.UserResponsibilityRepository
                .GetByCondition(e => e.UserId == model.Id && responsibilitiesToRemove.Contains(e.ResponsibilityId))
                .ToList();

            if (removedUserResponsibilities.Count > 0)
                repositoryManager.UserResponsibilityRepository.BulkDeleteIfExist(removedUserResponsibilities);
            if (addedUserResponsibilities.Count > 0)
                repositoryManager.UserResponsibilityRepository.BulkInsert(addedUserResponsibilities);

            await unitOfWork.SaveAsync();

            /*var getUserRolesFromDB = await userManagerRepository.GetRolesAsync(getUser);
            if ((model.Responsibilities == null || model.Responsibilities.Count == 0) && getUserRolesFromDB != null && getUserRolesFromDB.Count > 0)
            {
                var removeRolesResult = await userManagerRepository.RemoveFromRolesAsync(getUser, getUserRolesFromDB);
                if (!removeRolesResult.Succeeded)
                {
                    unitOfWork.Rollback();
                    throw new BusinessValidationException(LeillaKeys.SorryErrorHappenWhileUpdatingUser);
                }
            }

            if (model.Responsibilities != null)
            {
                var getUserRoles = await repositoryManager.RoleRepository
                    .Get(r => model.Responsibilities.Contains(r.Id))
                    .Select(r => r.Name)
                    .ToListAsync();

                if (getUserRoles == null || getUserRoles.Count == 0)
                    throw new BusinessValidationException(LeillaKeys.SorryYouMustEnterOneResponsibilityAtLeast);

                var getWillDeletedUserRoles = getUserRolesFromDB.Where(dbr => !getUserRoles.Contains(dbr)).ToList();
                if (getWillDeletedUserRoles != null && getWillDeletedUserRoles.Count > 0)
                {
                    var removeRolesResult = await userManagerRepository.RemoveFromRolesAsync(getUser, getWillDeletedUserRoles);
                    if (!removeRolesResult.Succeeded)
                    {
                        unitOfWork.Rollback();
                        throw new BusinessValidationException(LeillaKeys.SorryErrorHappenWhileUpdatingUser);
                    }
                }
                var getWillAddedUserRoles = getUserRoles.Where(dbr => !getUserRolesFromDB.Contains(dbr)).ToList();
                if (getWillAddedUserRoles != null && getWillAddedUserRoles.Count > 0)
                {
                    var addRolesResult = await userManagerRepository.AddToRolesAsync(getUser, getWillAddedUserRoles);
                    if (!addRolesResult.Succeeded)
                    {
                        unitOfWork.Rollback();
                        throw new BusinessValidationException(LeillaKeys.SorryErrorHappenWhileUpdatingUser);
                    }
                }
                await unitOfWork.SaveAsync();
            }*/

            #endregion

            #endregion

            #region Handle Response

            await unitOfWork.CommitAsync();
            return true;

            #endregion
        }
        public async Task<bool> AdminPanelUpdate(AdminPanelUpdateUserModel model)
        {
            #region Model Validation

            var updateUserModelValidator = new AdminPanelUpdateUserModelValidator();
            var updateUserModelValidatorResult = updateUserModelValidator.Validate(model);
            if (!updateUserModelValidatorResult.IsValid)
            {
                var error = updateUserModelValidatorResult.Errors.FirstOrDefault();
                throw new BusinessValidationException(error.ErrorMessage);
            }

            #endregion

            #region Business Validation

            await userBLValidation.AdminPanelUpdateValidation(model);

            #endregion

            unitOfWork.CreateTransaction();

            #region Upload Profile Image

            string imageName = null;
            if (model.ProfileImageFile != null && model.ProfileImageFile.Length > 0)
            {
                var result = await uploadBLC.UploadFile(model.ProfileImageFile, LeillaKeys.Users)
                    ?? throw new BusinessValidationException(LeillaKeys.SorryErrorHappenWhileUploadProfileImage);
                imageName = result.FileName;
            }

            #endregion

            #region Update User

            var getUser = await repositoryManager.UserRepository.GetEntityByConditionWithTrackingAsync(user => !user.IsDeleted
            && user.Id == model.Id && user.CompanyId == null && user.Type == AuthenticationType.AdminPanel);

            getUser.Name = model.Name;
            getUser.Email = model.Email;
            getUser.UserName = model.Email;
            getUser.IsActive = model.IsActive;
            getUser.IsAdmin = model.IsAdmin;
            getUser.ModifiedDate = DateTime.Now;
            getUser.ModifyUserId = requestInfo.UserId;
            getUser.ProfileImageName = !string.IsNullOrEmpty(imageName) ? imageName : !string.IsNullOrEmpty(model.ProfileImageName)
                ? getUser.ProfileImageName : null;

            var updateUserResponse = await userManagerRepository.UpdateAsync(getUser);

            if (!updateUserResponse.Succeeded)
            {
                await unitOfWork.RollbackAsync();
                throw new BusinessValidationException(LeillaKeys.SorryErrorHappenWhileUpdatingUser);
            }

            await unitOfWork.SaveAsync();

            #region Update Responsibilities

            var existDbList = repositoryManager.UserResponsibilityRepository
                    .GetByCondition(e => e.UserId == getUser.Id)
                    .ToList();

            var existingResponsibilityIds = existDbList.Select(e => e.ResponsibilityId).ToList();

            var addedUserResponsibilities = model.Responsibilities != null ? model.Responsibilities
                .Where(responsibilityId => !existingResponsibilityIds.Contains(responsibilityId))
                .Select(responsibilityId => new UserResponsibility
                {
                    UserId = model.Id,
                    ResponsibilityId = responsibilityId,
                    ModifyUserId = requestInfo.UserId,
                    ModifiedDate = DateTime.UtcNow
                }).ToList() : new List<UserResponsibility>();

            var responsibilitiesToRemove = existDbList
                .Where(ge => model.Responsibilities == null || !model.Responsibilities.Contains(ge.ResponsibilityId))
                .Select(ge => ge.ResponsibilityId)
                .ToList();

            var removedUserResponsibilities = repositoryManager.UserResponsibilityRepository
                .GetByCondition(e => e.UserId == model.Id && responsibilitiesToRemove.Contains(e.ResponsibilityId))
                .ToList();

            if (removedUserResponsibilities.Count > 0)
                repositoryManager.UserResponsibilityRepository.BulkDeleteIfExist(removedUserResponsibilities);
            if (addedUserResponsibilities.Count > 0)
                repositoryManager.UserResponsibilityRepository.BulkInsert(addedUserResponsibilities);

            await unitOfWork.SaveAsync();

            #endregion

            #endregion

            #region Handle Response

            await unitOfWork.CommitAsync();
            return true;

            #endregion
        }
        public async Task<GetUsersResponse> Get(GetUsersCriteria criteria)
        {
            var userRepository = repositoryManager.UserRepository;
            var query = userRepository.GetAsQueryable(criteria);

            #region paging

            int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            int take = PagingHelper.Take(criteria.PageSize);

            #region sorting

            var queryOrdered = userRepository.OrderBy(query, nameof(MyUser.Id), LeillaKeys.Desc);

            #endregion

            var queryPaged = criteria.GetPagingEnabled() ? queryOrdered.Skip(skip).Take(take) : queryOrdered;

            #endregion

            #region Handle Response

            var usersList = await queryPaged.Select(e => new GeUsersResponseModel
            {
                Id = e.Id,
                Code = e.Code,
                Name = e.Name,
                IsActive = e.IsActive,
                IsAdmin = e.IsAdmin,
                ProfileImagePath = uploadBLC.GetFilePath(e.ProfileImageName, LeillaKeys.Users)
            }).ToListAsync();

            return new GetUsersResponse
            {
                Users = usersList,
                TotalCount = await query.CountAsync()
            };

            #endregion

        }
        public async Task<GetUsersForDropDownResponse> GetForDropDown(GetUsersCriteria criteria)
        {
            criteria.IsActive = true;
            var userRepository = repositoryManager.UserRepository;
            var query = userRepository.GetAsQueryable(criteria);

            #region paging

            int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            int take = PagingHelper.Take(criteria.PageSize);

            #region sorting

            var queryOrdered = userRepository.OrderBy(query, nameof(MyUser.Id), LeillaKeys.Desc);

            #endregion

            var queryPaged = criteria.GetPagingEnabled() ? queryOrdered.Skip(skip).Take(take) : queryOrdered;

            #endregion

            #region Handle Response

            var usersList = await queryPaged.Select(e => new GetUsersForDropDownResponseModel
            {
                Id = e.Id,
                Name = e.Name
            }).ToListAsync();

            return new GetUsersForDropDownResponse
            {
                Users = usersList,
                TotalCount = await query.CountAsync()
            };

            #endregion

        }
        public async Task<GetUserInfoResponseModel> GetInfo(int userId)
        {
            var isArabic = requestInfo.Lang == LeillaKeys.Ar;

            var user = await repositoryManager.UserRepository.
                Get(user => user.Id == userId && !user.IsDeleted &&
                user.Type == AuthenticationType.DawemAdmin && user.CompanyId == requestInfo.CompanyId)
                .Select(user => new GetUserInfoResponseModel
                {
                    Code = user.Code,
                    Name = user.Name,
                    EmployeeName = user.Employee != null ? user.Employee.Name : null,
                    IsActive = user.IsActive,
                    IsAdmin = user.IsAdmin,
                    Email = user.Email,
                    MobileCountryCode = LeillaKeys.PlusSign + LeillaKeys.Space + user.MobileCountry.Dial,
                    MobileCountryName = isArabic ? user.MobileCountry.NameAr : user.MobileCountry.NameEn,
                    MobileCountryFlagPath = uploadBLC.GetFilePath(user.MobileCountry.Iso + LeillaKeys.PNG, LeillaKeys.AllCountriesFlags),
                    MobileNumber = user.MobileNumber,
                    ProfileImagePath = uploadBLC.GetFilePath(user.ProfileImageName, LeillaKeys.Users),
                    ProfileImageName = user.ProfileImageName,
                    Responsibilities = user.UserResponsibilities.Select(ur => ur.Responsibility.Name).ToList()
                }).FirstOrDefaultAsync() ?? throw new BusinessValidationException(LeillaKeys.SorryUserNotFound);
            return user;
        }
        public async Task<GetUserByIdResponseModel> GetById(int userId)
        {
            var user = await repositoryManager.UserRepository.
                Get(user => user.Id == userId && !user.IsDeleted &&
                user.Type == AuthenticationType.DawemAdmin && user.CompanyId == requestInfo.CompanyId)
                .Select(user => new GetUserByIdResponseModel
                {
                    Id = user.Id,
                    Code = user.Code,
                    Name = user.Name,
                    EmployeeId = user.EmployeeId,
                    IsActive = user.IsActive,
                    IsAdmin = user.IsAdmin,
                    Email = user.Email,
                    MobileCountryId = user.MobileCountryId ?? 0,
                    MobileNumber = user.MobileNumber,
                    ProfileImageName = user.ProfileImageName,
                    ProfileImagePath = uploadBLC.GetFilePath(user.ProfileImageName, LeillaKeys.Users),
                    Responsibilities = user.UserResponsibilities.Select(ur => ur.ResponsibilityId).ToList()
                }).FirstOrDefaultAsync() ?? throw new BusinessValidationException(LeillaKeys.SorryUserNotFound);

            return user;

        }
        public async Task<AdminPanelGetUserInfoResponseModel> AdminPanelGetInfo(int userId)
        {
            var isArabic = requestInfo.Lang == LeillaKeys.Ar;

            var user = await repositoryManager.UserRepository.
                Get(user => user.Id == userId && !user.IsDeleted &&
                   user.Type == AuthenticationType.AdminPanel && user.CompanyId == null)
                .Select(user => new AdminPanelGetUserInfoResponseModel
                {
                    Code = user.Code,
                    Name = user.Name,
                    IsActive = user.IsActive,
                    IsAdmin = user.IsAdmin,
                    Email = user.Email,
                    ProfileImagePath = uploadBLC.GetFilePath(user.ProfileImageName, LeillaKeys.Users),
                    ProfileImageName = user.ProfileImageName,
                    Responsibilities = user.UserResponsibilities.Select(ur => ur.Responsibility.Name).ToList()
                }).FirstOrDefaultAsync() ?? throw new BusinessValidationException(LeillaKeys.SorryUserNotFound);
            return user;
        }
        public async Task<AdminPanelGetUserByIdResponseModel> AdminPanelGetById(int userId)
        {
            var user = await repositoryManager.UserRepository.
                Get(user => user.Id == userId && !user.IsDeleted &&
                  user.Type == AuthenticationType.AdminPanel && user.CompanyId == null)
                .Select(user => new AdminPanelGetUserByIdResponseModel
                {
                    Id = user.Id,
                    Code = user.Code,
                    Name = user.Name,
                    IsActive = user.IsActive,
                    IsAdmin = user.IsAdmin,
                    Email = user.Email,
                    ProfileImageName = user.ProfileImageName,
                    ProfileImagePath = uploadBLC.GetFilePath(user.ProfileImageName, LeillaKeys.Users),
                    Responsibilities = user.UserResponsibilities.Select(ur => ur.ResponsibilityId).ToList()
                }).FirstOrDefaultAsync() ?? throw new BusinessValidationException(LeillaKeys.SorryUserNotFound);

            return user;

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
        public async Task<GetUsersInformationsResponseDTO> GetUsersInformations()
        {
            var userRepository = repositoryManager.UserRepository;
            var query = userRepository.Get(user => user.Type == requestInfo.Type &&
                ((requestInfo.CompanyId > 0 && user.CompanyId == requestInfo.CompanyId) ||
                (requestInfo.CompanyId <= 0 && user.CompanyId == null)));

            #region Handle Response

            return new GetUsersInformationsResponseDTO
            {
                TotalCount = await query.Where(user => !user.IsDeleted).CountAsync(),
                ActiveCount = await query.Where(user => !user.IsDeleted && user.IsActive).CountAsync(),
                NotActiveCount = await query.Where(user => !user.IsDeleted && !user.IsActive).CountAsync(),
                DeletedCount = await query.Where(user => user.IsDeleted).CountAsync()
            };

            #endregion
        }

        public async Task<string> GetUserNameByEmployeeId(int employeeId)
        {
            #region Get User Name

            var getUserName = await repositoryManager.EmployeeRepository
                .Get(e => e.CompanyId == requestInfo.CompanyId && e.Id == employeeId)
                .Select(e => e.Email)
                .FirstOrDefaultAsync();

            if (getUserName == null)
            {
                throw new BusinessValidationException(AmgadKeys.SorryCannotConnectThisEmployeeWithUserBecausehisEmailIsNull);
            }
            return getUserName;

            #endregion
        }
    }
}

