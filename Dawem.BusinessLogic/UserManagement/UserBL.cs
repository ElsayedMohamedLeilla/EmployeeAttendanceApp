using AutoMapper;
using Dawem.Contract.BusinessLogic.Employees;
using Dawem.Contract.BusinessLogic.Provider;
using Dawem.Contract.BusinessLogicCore;
using Dawem.Contract.BusinessValidation.Employees;
using Dawem.Contract.Repository.Manager;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.UserManagement;
using Dawem.Helpers;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Employees.User;
using Dawem.Models.Dtos.Shared;
using Dawem.Models.Exceptions;
using Dawem.Models.Response.Employees.User;
using Dawem.Repository.UserManagement;
using Dawem.Translations;
using Dawem.Validation.FluentValidation.Employees.User;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Dawem.BusinessLogic.Employees
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
            #region Business Validation

            var employeeId = await userBLValidation.SignUpValidation(model);

            #endregion

            unitOfWork.CreateTransaction();

            #region Insert User

            #region Set User code And Verification Code

            var getNextCode = await repositoryManager.UserRepository
                .Get(e => e.CompanyId == model.CompanyId)
                .Select(e => e.Code)
                .DefaultIfEmpty()
                .MaxAsync() + 1;

            string getNewVerificationCode = await GetVerificationCode(model.CompanyId);

            #endregion

            var user = mapper.Map<MyUser>(model);
            user.UserName = model.Email;
            user.Code = getNextCode;
            user.EmployeeId = employeeId;
            user.VerificationCode = getNewVerificationCode;
            user.VerificationCodeSendDate = DateTime.UtcNow;
            user.IsActive = true;

            var createUserResponse = await userManagerRepository.CreateAsync(user, model.Password);
            if (!createUserResponse.Succeeded)
            {
                unitOfWork.Rollback();
                throw new BusinessValidationException(LeillaKeys.SorryErrorHappenWhileAddingUser);
            }

            var Roles = new List<string>() { LeillaKeys.RoleEMPLOYEE, LeillaKeys.RoleUSER };
            var assignRolesResult = await userManagerRepository.AddToRolesAsync(user, Roles);
            if (!assignRolesResult.Succeeded)
            {
                unitOfWork.Rollback();
                throw new BusinessValidationException(LeillaKeys.SorryErrorHappenWhileAddingUser);
            }
            await unitOfWork.SaveAsync();


            #endregion

            #region Send Verification Code In Email

            var verifyEmail = new VerifyEmailModel
            {
                Email = user.Email,
                Subject = TranslationHelper.GetTranslation(LeillaKeys.ThanksForRegistrationOnDawem, requestInfo?.Lang),
                Body = TranslationHelper.GetTranslation(LeillaKeys.YouAreDoneRegistrationSuccessfullyOnDawemYouMustEnterThisVerificationCodeOnDawemToVerifyYourEmailAndCanSignIn, requestInfo?.Lang)
                + getNewVerificationCode
            };

            await mailBL.SendEmail(verifyEmail);

            #endregion

            #region Handle Response

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
        private async Task<string> GetVerificationCode(int companyId)
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

            #region Upload Profile Image

            string imageName = null;
            if (model.ProfileImageFile != null && model.ProfileImageFile.Length > 0)
            {
                var result = await uploadBLC.UploadImageFile(model.ProfileImageFile, LeillaKeys.Users)
                    ?? throw new BusinessValidationException(LeillaKeys.SorryErrorHappenWhileUploadProfileImage); ;
                imageName = result.FileName;
            }

            #endregion

            #region Insert User

            #region Set User code

            var getNextCode = await repositoryManager.UserRepository
                .Get(e => e.CompanyId == requestInfo.CompanyId)
                .Select(e => e.Code)
                .DefaultIfEmpty()
                .MaxAsync() + 1;

            #endregion

            var user = mapper.Map<MyUser>(model);
            user.CompanyId = requestInfo.CompanyId;
            user.AddUserId = requestInfo.UserId;
            user.UserName = model.Email;
            user.ProfileImageName = imageName;
            user.Code = getNextCode;

            var createUserResponse = await userManagerRepository.CreateAsync(user, model.Password);
            if (!createUserResponse.Succeeded)
            {
                unitOfWork.Rollback();
                throw new BusinessValidationException(LeillaKeys.SorryErrorHappenWhileAddingUser);
            }

            if (model.Roles != null)
            {
                var assignRolesResult = await userManagerRepository.AddToRolesAsync(user, model.Roles);
                if (!assignRolesResult.Succeeded)
                {
                    unitOfWork.Rollback();
                    throw new BusinessValidationException(LeillaKeys.SorryErrorHappenWhileAddingUser);
                }
                await unitOfWork.SaveAsync();
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

            #region Upload Profile Image

            string imageName = null;
            if (model.ProfileImageFile != null && model.ProfileImageFile.Length > 0)
            {
                var result = await uploadBLC.UploadImageFile(model.ProfileImageFile, LeillaKeys.Users)
                    ?? throw new BusinessValidationException(LeillaKeys.SorryErrorHappenWhileUploadProfileImage);
                imageName = result.FileName;
            }

            #endregion

            #region Update User

            var getUser = await repositoryManager.UserRepository.GetEntityByConditionWithTrackingAsync(user => !user.IsDeleted
            && user.Id == model.Id);

            getUser.Name = model.Name;
            getUser.EmployeeId = model.EmployeeId;
            getUser.Email = model.Email;
            getUser.UserName = model.Email;
            getUser.MobileNumber = model.MobileNumber;
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

            #region Update Roles

            var getUserRolesFromDB = await userManagerRepository.GetRolesAsync(getUser);
            if ((model.Roles == null || model.Roles.Count == 0) && getUserRolesFromDB != null && getUserRolesFromDB.Count > 0)
            {
                var removeRolesResult = await userManagerRepository.RemoveFromRolesAsync(getUser, getUserRolesFromDB);
                if (!removeRolesResult.Succeeded)
                {
                    unitOfWork.Rollback();
                    throw new BusinessValidationException(LeillaKeys.SorryErrorHappenWhileUpdatingUser);
                }
            }
            if (model.Roles != null)
            {
                var getWillDeletedUserRoles = getUserRolesFromDB.Where(dbr => !model.Roles.Contains(dbr)).ToList();
                if (getWillDeletedUserRoles != null && getWillDeletedUserRoles.Count > 0)
                {
                    var removeRolesResult = await userManagerRepository.RemoveFromRolesAsync(getUser, getWillDeletedUserRoles);
                    if (!removeRolesResult.Succeeded)
                    {
                        unitOfWork.Rollback();
                        throw new BusinessValidationException(LeillaKeys.SorryErrorHappenWhileUpdatingUser);
                    }
                }
                var getWillAddedUserRoles = model.Roles.Where(dbr => !getUserRolesFromDB.Contains(dbr)).ToList();
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
            }

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

            var queryPaged = criteria.PagingEnabled ? queryOrdered.Skip(skip).Take(take) : queryOrdered;

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

            var queryPaged = criteria.PagingEnabled ? queryOrdered.Skip(skip).Take(take) : queryOrdered;

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
            var user = await repositoryManager.UserRepository.Get(e => e.Id == userId && !e.IsDeleted)
                .Select(user => new GetUserInfoResponseModel
                {
                    Code = user.Code,
                    Name = user.Name,
                    EmployeeName = user.Employee != null ? user.Employee.Name : null,
                    IsActive = user.IsActive,
                    IsAdmin = user.IsAdmin,
                    Email = user.Email,
                    MobileNumber = user.MobileNumber,
                    ProfileImagePath = uploadBLC.GetFilePath(user.ProfileImageName, LeillaKeys.Users),
                    Roles = user.UserRoles.Select(ur => TranslationHelper.GetTranslation(ur.Role.Name, requestInfo.Lang)).ToList()
                }).FirstOrDefaultAsync() ?? throw new BusinessValidationException(LeillaKeys.SorryUserNotFound);
            return user;
        }
        public async Task<GetUserByIdResponseModel> GetById(int userId)
        {
            var user = await repositoryManager.UserRepository.Get(e => e.Id == userId && !e.IsDeleted)
                .Select(user => new GetUserByIdResponseModel
                {
                    Id = user.Id,
                    Code = user.Code,
                    Name = user.Name,
                    EmployeeId = user.EmployeeId,
                    IsActive = user.IsActive,
                    Email = user.Email,
                    MobileNumber = user.MobileNumber,
                    ProfileImageName = user.ProfileImageName,
                    ProfileImagePath = uploadBLC.GetFilePath(user.ProfileImageName, LeillaKeys.Users),
                    Roles = user.UserRoles.Select(ur => ur.Role.Name).ToList()
                }).FirstOrDefaultAsync() ?? throw new BusinessValidationException(LeillaKeys.SorryUserNotFound);

            return user;

        }
        public async Task<bool> Delete(int userId)
        {
            MyUser user = await repositoryManager.UserRepository.GetEntityByConditionWithTrackingAsync(d => !d.IsDeleted && d.Id == userId) ??
                throw new BusinessValidationException(LeillaKeys.SorryUserNotFound);
            user.Delete();
            await unitOfWork.SaveAsync();
            return true;
        }
    }
}

