using AutoMapper;
using Dawem.Contract.BusinessLogic.Provider;
using Dawem.Contract.BusinessLogic.UserManagement;
using Dawem.Contract.BusinessValidation;
using Dawem.Contract.Repository.Manager;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Core;
using Dawem.Domain.Entities.Provider;
using Dawem.Domain.Entities.UserManagement;
using Dawem.Enums.General;
using Dawem.Helpers;
using Dawem.Models.Context;
using Dawem.Models.Criteria.UserManagement;
using Dawem.Models.Dtos.Identity;
using Dawem.Models.Dtos.Shared;
using Dawem.Models.DtosMappers;
using Dawem.Models.Exceptions;
using Dawem.Models.ResponseModels;
using Dawem.Repository.UserManagement;
using Dawem.Translations;
using Dawem.Validation.FluentValidation.Authentication;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Dawem.BusinessLogic.UserManagement
{
    public class UserBLOld : IUserBLOld
    {
        private readonly IUnitOfWork<ApplicationDBContext> unitOfWork;
        private readonly UserManagerRepository userManagerRepository;
        private readonly RequestInfo requestHeaderContext;
        private readonly IBranchBLValidation branchValidatorBL;
        private readonly IRepositoryManager repositoryManager;
        private readonly IUserBranchBL userBranchBL;
        private readonly IMapper mapper;
        private readonly IUserOldBLValidation userBLValidation;

        public UserBLOld(IUnitOfWork<ApplicationDBContext> _unitOfWork, IRepositoryManager _repositoryManager,
            UserManagerRepository _smartUserManagerRepository, IMapper _mapper,
            IConfiguration _config, RequestInfo _userContext,
            IBranchBLValidation _branchValidatorBL, IUserOldBLValidation _userBLValidation,
             IUserBranchBL _userBranchBL)
        {
            unitOfWork = _unitOfWork;
            userManagerRepository = _smartUserManagerRepository;
            requestHeaderContext = _userContext;
            branchValidatorBL = _branchValidatorBL;
            userBranchBL = _userBranchBL;
            repositoryManager = _repositoryManager;
            mapper = _mapper;
            userBLValidation = _userBLValidation;
        }

        public async Task<GetUsersResponseModelOld> Get(UserSearchCriteria criteria)
        {
            var query = repositoryManager.UserRepository.GetAsQueryableOld(criteria, nameof(MyUser.UserBranches));
            var queryOrdered = repositoryManager.UserRepository.OrderBy(query, nameof(MyUser.Id), DawemKeys.Desc);

            #region paging

            int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            int take = PagingHelper.Take(criteria.PageSize);
            var queryPaged = criteria.PagingEnabled ? queryOrdered.Skip(skip).Take(take) : queryOrdered;

            #endregion

            var users = await queryPaged.ToListAsync();

            UserDTOMapper.InitUserContext(requestHeaderContext);

            var usersList = UserDTOMapper.MapListUsers(users);

            return new GetUsersResponseModelOld { Users = usersList, TotalCount = await query.CountAsync() };

        }
        public async Task<UserInfo> GetInfo(GetUserInfoCriteria criteria)
        {
            var user = await repositoryManager.UserRepository
            .GetEntityByConditionWithTrackingAsync(u => u.Id == criteria.Id,
            nameof(MyUser.UserBranches) + DawemKeys.Comma + nameof(MyUser.UserBranches) + DawemKeys.Dot + nameof(UserBranch.Branch) +
             DawemKeys.Comma + nameof(MyUser.UserGroups) + DawemKeys.Comma + nameof(MyUser.UserGroups) + DawemKeys.Dot + nameof(UserGroup.Group)) ??
             throw new BusinessValidationException(DawemKeys.SorryUserNotFound);

            UserDTOMapper.InitUserContext(requestHeaderContext);
            var userInfo = UserDTOMapper.MapInfo(user);

            return userInfo;
        }
        public async Task<int> Create(CreatedUser createdUser)
        {
            MyUser user = mapper.Map<MyUser>(createdUser);
            user.BranchId = requestHeaderContext.BranchId;
            user.EmailConfirmed = true;
            user.UserName = user.Email;

            #region Model Validation

            var userValidator = new UserValidator();
            var userValidatorResult = userValidator.Validate(createdUser);
            if (!userValidatorResult.IsValid)
            {
                var error = userValidatorResult.Errors.FirstOrDefault();
                throw new BusinessValidationException(error.ErrorMessage);
            }

            #endregion

            branchValidatorBL.ValidateChangeForMainBranchOnly(requestHeaderContext, ChangeType.Add);
            await userBLValidation.CreateUserValidation(createdUser);

            await unitOfWork.CreateTransactionAsync();

            IdentityResult createUserResponse = await userManagerRepository.CreateAsync(user, createdUser.Password);
            if (!createUserResponse.Succeeded)
            {
                throw new BusinessValidationException(DawemKeys.SorryErrorHappenWhileAddingUser);
            }
            IdentityResult addingToRoleResult = new();
            if (createdUser.UserRols == null || createdUser.UserRols.Count == 0)
            {
                addingToRoleResult = await userManagerRepository.AddToRoleAsync(user, DawemKeys.FullAccess);

            }
            else
            {
                addingToRoleResult = await userManagerRepository.AddToRolesAsync(user, createdUser.UserRols.ToArray());
            }

            if (!addingToRoleResult.Succeeded)
            {
                unitOfWork.Rollback();
                await userManagerRepository.DeleteAsync(user);
                throw new BusinessValidationException(DawemKeys.SorryErrorHappenWhileAddingUser);
            }

            #region User Branches

            var userBranches = createdUser.UserBranches;

            foreach (var item in userBranches)
            {
                item.UserId = user.Id;
            }
            repositoryManager.UserBranchRepository.BulkInsert(userBranches);
            await unitOfWork.SaveAsync();

            #endregion

            #region User Groups

            var userGroups = createdUser.UserGroups;

            foreach (var item in userGroups)
            {
                item.UserId = user.Id;
            }
            repositoryManager.UserGroupRepository.BulkInsert(userGroups);
            await unitOfWork.SaveAsync();

            #endregion

            await unitOfWork.CommitAsync();
            return user.Id;

        }
        public async Task<bool> Update(CreatedUser updatedUser)
        {
            updatedUser.MainBranchId = requestHeaderContext.BranchId ?? 0;
            MyUser user = mapper.Map<MyUser>(updatedUser);

            #region Model Validation

            var userValidator = new UserValidator();
            var userValidatorResult = userValidator.Validate(updatedUser);
            if (!userValidatorResult.IsValid)
            {
                var error = userValidatorResult.Errors.FirstOrDefault();
                throw new BusinessValidationException(error.ErrorMessage);
            }

            #endregion

            branchValidatorBL.ValidateChangeForMainBranchOnly(requestHeaderContext, ChangeType.Edit);
            await userBLValidation.CreateUserValidation(updatedUser);

            await unitOfWork.CreateTransactionAsync();

            var user2 = await userManagerRepository.FindByIdAsync(updatedUser.Id.ToString());

            user2.Email = user.Email;
            user2.Name = user.Name;
            user2.IsActive = user.IsActive;
            user2.Gender = user.Gender;
            user2.BirthDate = user.BirthDate;
            user2.PhoneNumber = user.PhoneNumber;
            user2.MobileNumber = user.MobileNumber;

            var updateUserResponse = await userManagerRepository.UpdateAsync(user2);

            if (!updateUserResponse.Succeeded)
            {
                throw new BusinessValidationException(DawemKeys.SorryErrorHappenWhileUpdatingUser);
            }

            user = await userManagerRepository.FindByIdAsync(updatedUser.Id.ToString());

            #region User Branches

            var UserBranches = updatedUser.UserBranches;

            user.UserBranches = null;

            var updatedBranches = new List<UserBranch>();
            var DBUserBranches = await userBranchBL.GetByUser(user.Id);

            if (DBUserBranches != null)
            {
                var FindWillDeletedUserBranches = DBUserBranches.Where(dbss => !UserBranches.Any(ss => ss.BranchId == dbss.BranchId))
                    .ToList();

                foreach (var willDeletedUserBranche in FindWillDeletedUserBranches)
                {
                    repositoryManager.UserBranchRepository.Delete(willDeletedUserBranche.Id);
                }

                await unitOfWork.SaveAsync();
            }

            if (UserBranches != null)
            {
                foreach (var item in updatedUser.UserBranches)
                {
                    item.UserId = user.Id;
                    if (item.Id > 0)
                    {
                        repositoryManager.UserBranchRepository.Update(item);
                    }
                    else
                    {
                        repositoryManager.UserBranchRepository.Insert(item);
                    }
                };

                await unitOfWork.SaveAsync();
            }

            #endregion

            #region User Groups

            var UserGroups = updatedUser.UserGroups;

            user.UserGroups = null;

            var updatedUserGroups = new List<UserBranch>();
            var DBUserGroups = repositoryManager.UserGroupRepository.Get(u => u.UserId == user.Id).ToList();

            if (DBUserGroups != null)
            {

                var FindWillDeletedUserGroups = DBUserGroups.Where(dbss => !UserGroups.Any(ss => ss.GroupId == dbss.GroupId))
                    .ToList();

                foreach (var willDeletedUserGroup in FindWillDeletedUserGroups)
                {
                    repositoryManager.UserGroupRepository.Delete(willDeletedUserGroup.Id);
                }

                await unitOfWork.SaveAsync();
            }

            if (UserGroups != null)
            {
                foreach (var item in updatedUser.UserGroups)
                {
                    item.UserId = user.Id;
                    if (item.Id > 0)
                    {
                        repositoryManager.UserGroupRepository.Update(item);
                    }
                    else
                    {
                        repositoryManager.UserGroupRepository.Insert(item);
                    }
                };
                await unitOfWork.SaveAsync();
            }

            #endregion

            await unitOfWork.CommitAsync();
            return true;
        }
        public async Task<bool> DeleteById(int userId)
        {
            branchValidatorBL.ValidateChangeForMainBranchOnly(requestHeaderContext, ChangeType.Delete);
            var myUser = await repositoryManager.UserRepository.Get(a => a.Id == userId).FirstOrDefaultAsync();

            if (myUser != null)
            {
                await unitOfWork.CreateTransactionAsync();
                var userbranches = repositoryManager.UserBranchRepository.Get(a => a.UserId == userId).ToList();

                foreach (var item in userbranches)
                {
                    repositoryManager.UserBranchRepository.Delete(item);
                }

                var userRoles = repositoryManager.UserRoleRepository.Get(a => a.UserId == userId).ToList();

                foreach (var item in userRoles)
                {
                    repositoryManager.UserRoleRepository.Delete(item);
                }

                repositoryManager.UserRepository.Delete(myUser);

                await unitOfWork.SaveAsync();
                await unitOfWork.CommitAsync();
            }
            return true;
        }
        public async Task<bool> IsEmailUnique(ValidationItems validationItem)
        {
            MyUser duplicateUser = null;
            if (string.IsNullOrEmpty(validationItem.Item))
            {
                throw new ArgumentNullException(nameof(validationItem));
            }
            if (validationItem.validationMode == ValidationMode.Create)
            {
                duplicateUser = await repositoryManager.UserRepository.Get(x => x.Email.ToLower().Trim() == validationItem.Item.ToLower().Trim())
                    .FirstOrDefaultAsync();
            }

            else if (validationItem.validationMode == ValidationMode.Update && validationItem.Id != null)
            {
                duplicateUser = await repositoryManager.UserRepository
                    .Get(x => x.Email.ToLower() == validationItem.Item.ToLower() && x.Id != validationItem.Id.Value)
                   .FirstOrDefaultAsync();
            }

            if (duplicateUser == null)
            {
                return true;
            }
            else
            {
                return false;

            }
        }
    }
}

