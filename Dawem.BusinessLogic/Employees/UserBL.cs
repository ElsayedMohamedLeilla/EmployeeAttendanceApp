using AutoMapper;
using Dawem.Contract.BusinessLogic.Employees;
using Dawem.Contract.BusinessLogicCore;
using Dawem.Contract.BusinessValidation.Employees;
using Dawem.Contract.Repository.Manager;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.UserManagement;
using Dawem.Helpers;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Employees.User;
using Dawem.Models.Exceptions;
using Dawem.Models.Response.Employees.Employee;
using Dawem.Models.Response.Employees.User;
using Dawem.Repository.UserManagement;
using Dawem.Translations;
using Dawem.Validation.FluentValidation.Employees;
using Dawem.Validation.FluentValidation.Employees.Employees;
using Microsoft.EntityFrameworkCore;

namespace Dawem.BusinessLogic.Users
{
    public class UserBL : IUserBL
    {
        private readonly IUnitOfWork<ApplicationDBContext> unitOfWork;
        private readonly RequestInfo requestInfo;
        private readonly IUserBLValidation userBLValidation;
        private readonly IRepositoryManager repositoryManager;
        private readonly IMapper mapper;
        private readonly IUploadBLC uploadBLC;
        private readonly UserManagerRepository userManagerRepository;
        public UserBL(IUnitOfWork<ApplicationDBContext> _unitOfWork,
            IRepositoryManager _repositoryManager,
            IMapper _mapper,
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
            uploadBLC = _uploadBLC;
            userManagerRepository = _userManagerRepository;
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
                var result = await uploadBLC.UploadImageFile(model.ProfileImageFile, DawemKeys.Users)
                    ?? throw new BusinessValidationException(DawemKeys.SorryErrorHappenWhileUploadProfileImage); ;
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

            string RoleName = DawemKeys.FullAccess;

            var createUserResponse = await userManagerRepository.CreateAsync(user, model.Password);
            if (!createUserResponse.Succeeded)
            {
                unitOfWork.Rollback();
                throw new BusinessValidationException(DawemKeys.SorryErrorHappenWhileAddingUser);
            }

            var assignRole = await userManagerRepository.AddToRoleAsync(user, RoleName);
            if (!assignRole.Succeeded)
            {
                unitOfWork.Rollback();
                throw new BusinessValidationException(DawemKeys.SorryErrorHappenWhileAddingUser);
            }
            await unitOfWork.SaveAsync();

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
                var result = await uploadBLC.UploadImageFile(model.ProfileImageFile, DawemKeys.Users)
                    ?? throw new BusinessValidationException(DawemKeys.SorryErrorHappenWhileUploadProfileImage);
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
            getUser.ProfileImageName = !string.IsNullOrEmpty(imageName) ? imageName : !string.IsNullOrEmpty(model.ProfileImageName)
                ? getUser.ProfileImageName : null;

            var updateUserResponse = await userManagerRepository.UpdateAsync(getUser);

            if (!updateUserResponse.Succeeded)
            {
                await unitOfWork.RollbackAsync();
                throw new BusinessValidationException(DawemKeys.SorryErrorHappenWhileUpdatingUser);
            }

            await unitOfWork.SaveAsync();

            #endregion

            #region Handle Response

            await unitOfWork.CommitAsync();
            return true;

            #endregion
        }
        public async Task<GetUsersResponse> Get(GetUsersCriteria criteria)
        {
            #region Model Validation

            var getValidator = new GetGenaricValidator();
            var getValidatorResult = getValidator.Validate(criteria);
            if (!getValidatorResult.IsValid)
            {
                var error = getValidatorResult.Errors.FirstOrDefault();
                throw new BusinessValidationException(error.ErrorMessage);
            }

            #endregion

            var userRepository = repositoryManager.UserRepository;
            var query = userRepository.GetAsQueryable(criteria);

            #region paging

            int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            int take = PagingHelper.Take(criteria.PageSize);

            #region sorting

            var queryOrdered = userRepository.OrderBy(query, nameof(MyUser.Id), DawemKeys.Desc);

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
                ProfileImagePath = uploadBLC.GetFilePath(e.ProfileImageName, DawemKeys.Users)
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
            #region Model Validation

            var getValidator = new GetGenaricValidator();
            var getValidatorResult = getValidator.Validate(criteria);
            if (!getValidatorResult.IsValid)
            {
                var error = getValidatorResult.Errors.FirstOrDefault();
                throw new BusinessValidationException(error.ErrorMessage);
            }

            #endregion

            criteria.IsActive = true;
            var userRepository = repositoryManager.UserRepository;
            var query = userRepository.GetAsQueryable(criteria);

            #region paging

            int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            int take = PagingHelper.Take(criteria.PageSize);

            #region sorting

            var queryOrdered = userRepository.OrderBy(query, nameof(MyUser.Id), DawemKeys.Desc);

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
                .Select(e => new GetUserInfoResponseModel
                {
                    Code = e.Code,
                    Name = e.Name,
                    EmployeeName = e.Employee != null ? e.Employee.Name : null,
                    IsActive = e.IsActive,
                    IsAdmin = e.IsAdmin,
                    Email = e.Email,
                    MobileNumber = e.MobileNumber,
                    ProfileImagePath = uploadBLC.GetFilePath(e.ProfileImageName, DawemKeys.Users)
                }).FirstOrDefaultAsync() ?? throw new BusinessValidationException(DawemKeys.SorryUserNotFound);

            return user;
        }
        public async Task<GetUserByIdResponseModel> GetById(int userId)
        {
            var user = await repositoryManager.UserRepository.Get(e => e.Id == userId && !e.IsDeleted)
                .Select(e => new GetUserByIdResponseModel
                {
                    Id = e.Id,
                    Code = e.Code,
                    Name = e.Name,
                    EmployeeId = e.EmployeeId,
                    IsActive = e.IsActive,
                    Email = e.Email,
                    MobileNumber = e.MobileNumber,
                    ProfileImageName = e.ProfileImageName,
                    ProfileImagePath = uploadBLC.GetFilePath(e.ProfileImageName, DawemKeys.Users)
                }).FirstOrDefaultAsync() ?? throw new BusinessValidationException(DawemKeys.SorryUserNotFound);

            return user;

        }
        public async Task<bool> Delete(int userId)
        {
            MyUser user = await repositoryManager.UserRepository.GetEntityByConditionWithTrackingAsync(d => !d.IsDeleted && d.Id == userId) ??
                throw new BusinessValidationException(DawemKeys.SorryUserNotFound);
            user.Delete();
            await unitOfWork.SaveAsync();
            return true;
        }
    }
}

