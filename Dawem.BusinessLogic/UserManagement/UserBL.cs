using AutoMapper;
using Dawem.Contract.BusinessLogic.UserManagement;
using Dawem.Contract.Repository.Core;
using Dawem.Contract.Repository.Provider;
using Dawem.Contract.Repository.UserManagement;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.UserManagement;
using Dawem.Enums.General;
using Dawem.Helpers;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Identity;
using Dawem.Models.Response;
using Dawem.Models.Response.Identity;
using Dawem.Repository.UserManagement;
using FluentValidation.Results;
using LinqKit;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SmartBusinessERP.Areas.Identity.Data.UserManagement;
using SmartBusinessERP.BusinessLogic.Provider.Contract;
using SmartBusinessERP.BusinessLogic.UserManagement.Contract;
using SmartBusinessERP.BusinessLogic.Validators.Contract;
using SmartBusinessERP.BusinessLogic.Validators.FluentValidators;
using SmartBusinessERP.Data;
using SmartBusinessERP.Data.UnitOfWork;
using SmartBusinessERP.Domain.Entities.Provider;
using SmartBusinessERP.Enums;
using SmartBusinessERP.Helpers;
using SmartBusinessERP.Models.Context;
using SmartBusinessERP.Models.Criteria.UserManagement;
using SmartBusinessERP.Models.Dtos.Identity;
using SmartBusinessERP.Models.Dtos.Shared;
using SmartBusinessERP.Models.DtosMappers;
using SmartBusinessERP.Models.Response;
using SmartBusinessERP.Models.Response.Identity;
using SmartBusinessERP.Repository.Core.Conract;
using SmartBusinessERP.Repository.Provider.Contract;
using SmartBusinessERP.Repository.UserManagement;
using SmartBusinessERP.Repository.UserManagement.Contract;

namespace SmartBusinessERP.BusinessLogic.UserManagement
{
    public class UserBL : IUserBL
    {
        private readonly IUnitOfWork<ApplicationDBContext> unitOfWork;
        private readonly UserManagerRepository smartUserManagerRepository;
        private readonly IUserRepository smartUserRepository;
        private readonly RequestHeaderContext userContext;
        private readonly IBranchValidatorBL branchValidatorBL;
        private readonly IBranchRepository branchRepository;
        private readonly IUserRoleRepository smartUserRoleRepository;
        private readonly IUserBranchRepository userBranchRepository;
        private readonly IUserGroupRepository userGroupRepository;
        private readonly IUserBranchBL userBranchBL;

        public UserBL(IUnitOfWork<ApplicationDBContext> _unitOfWork, IUserGroupRepository _userGroupRepository, IUserRoleRepository _smartUserRoleRepository, IUserRepository _smartUserRepository, UserManagerRepository _smartUserManagerRepository,
            IConfiguration _config, RequestHeaderContext _userContext, IBranchValidatorBL _branchValidatorBL, IBranchRepository _branchRepository, IUserBranchRepository _userBranchRepository, IUserBranchBL _userBranchBL)
        {
            unitOfWork = _unitOfWork;
            smartUserManagerRepository = _smartUserManagerRepository;
            smartUserRepository = _smartUserRepository;
            userContext = _userContext;
            branchValidatorBL = _branchValidatorBL;
            branchRepository = _branchRepository;
            userBranchRepository = _userBranchRepository;
            userGroupRepository = _userGroupRepository;
            smartUserRoleRepository = _smartUserRoleRepository;
            userBranchBL = _userBranchBL;

        }

        public async Task<UserSearchResult> Get(UserSearchCriteria criteria)
        {

            UserSearchResult userSearchResult = new()
            {
                Status = ResponseStatus.Success
            };
            try
            {
                ExpressionStarter<User> userPredicate = PredicateBuilder.New<User>(true);

                if (userContext.IsMainBranch && criteria.ForGridView)
                {

                    userPredicate = userPredicate.And(x => x.MainBranchId == userContext.BranchId);
                }
                else
                {
                    userPredicate = userPredicate.And(x => x.UserBranches.Any(a => a.BranchId == userContext.BranchId));
                }



                if (criteria.Id is not null)
                {
                    userPredicate = userPredicate.And(x => x.Id == criteria.Id);
                }


                if (!string.IsNullOrWhiteSpace(criteria.FreeText))
                {
                    criteria.FreeText = criteria.FreeText.ToLower().Trim();

                    userPredicate = userPredicate.Start(x => x.UserName.ToLower().Trim().Contains(criteria.FreeText));
                    userPredicate = userPredicate.Or(x => x.FirstName.ToLower().Trim().Contains(criteria.FreeText));
                    userPredicate = userPredicate.Or(x => x.LastName.ToLower().Trim().Contains(criteria.FreeText));
                    userPredicate = userPredicate.Or(x => x.Email.ToLower().Trim().Contains(criteria.FreeText));
                    userPredicate = userPredicate.Or(x => x.MobileNumber.ToLower().Trim().Contains(criteria.FreeText));
                    userPredicate = userPredicate.Or(x => x.PhoneNumber.ToLower().Trim().Contains(criteria.FreeText));
                }

                if (!string.IsNullOrWhiteSpace(criteria.UserName))
                {
                    userPredicate = userPredicate.And(x => x.UserName.ToLower().Trim().Contains(criteria.UserName.ToLower().Trim()));
                }



                if (criteria.IsActive != null)
                {
                    userPredicate = userPredicate.And(x => x.IsActive == true);
                }



                #region paging

                int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
                int take = PagingHelper.Take(criteria.PageSize);

                IQueryable<User> users = smartUserRepository.Get(userPredicate, IncludeProperties: !userContext.IsMainBranch && !criteria.ForGridView ? "UserBranches" : "");

                #region sorting
                IQueryable<User> usersOrdered = smartUserRepository.OrderBy(users, "Id", "desc");
                #endregion

                IQueryable<User> queryOrdered = usersOrdered;


                IQueryable<User> queryPaged = criteria.PagingEnabled ? queryOrdered.Skip(skip).Take(take) : queryOrdered;

                #endregion

                List<User> result = await queryPaged.ToListAsync();

                UserDTOMapper.InitUserContext(userContext);

                userSearchResult.Users = UserDTOMapper.MapListUsers(result);
                userSearchResult.TotalCount = queryOrdered.ToList().Count;
                userSearchResult.Status = ResponseStatus.Success;
            }
            catch (Exception ex)
            {
                userSearchResult.Exception = ex;
                userSearchResult.Status = ResponseStatus.Error;
            }
            return userSearchResult;

        }
        public async Task<GetUserInfoResponse> GetInfo(GetUserInfoCriteria criteria)
        {

            GetUserInfoResponse userSearchResult = new()
            {
                Status = ResponseStatus.Success
            };
            try
            {
                var user = await smartUserRepository.GetEntityByConditionWithTrackingAsync(u => u.Id == criteria.Id, "UserBranches,UserBranches.Branch,UserGroups,UserGroups.Group");

                if (user != null)
                {
                    UserDTOMapper.InitUserContext(userContext);

                    var userInfo = UserDTOMapper.MapInfo(user);
                    userSearchResult.UserInfo = userInfo;
                    userSearchResult.Status = ResponseStatus.Success;
                }
                else
                {
                    userSearchResult.Status = ResponseStatus.ValidationError;
                    TranslationHelper
                    .SetResponseMessages
                        (userSearchResult, "UserNotFound!",
                        "User Not Found !", lang: userContext.Lang);
                }


            }
            catch (Exception ex)
            {
                userSearchResult.Exception = ex;
                userSearchResult.Status = ResponseStatus.Error;
            }
            return userSearchResult;

        }
        public async Task<BaseResponseT<CreatedUser>> Create(CreatedUser createdUser)
        {

            BaseResponseT<CreatedUser> response = new()
            {

            };

            User smartUser = mapper.Map<User>(createdUser);
            smartUser.MainBranchId = userContext.BranchId;
            smartUser.EmailConfirmed = true;
            smartUser.UserName = smartUser.Email;
            UserValidator uservalidate = new(ValidationMode.Create, this, userContext);

            ValidationResult validation = uservalidate.Validate(smartUser);


            if (!validation.IsValid)
            {
                response.Status = ResponseStatus.ValidationError;
                response.Message = validation.Errors[0].ErrorMessage;
                response.MessageCode = validation.Errors[0].ErrorCode;

                return response;

            }
            var ValidateChangeForMainBranchOnlyResult = branchValidatorBL.ValidateChangeForMainBranchOnly(userContext, ChangeType.Add);

            if (ValidateChangeForMainBranchOnlyResult.Status != ResponseStatus.Success)
            {
                TranslationHelper.MapBaseResponse(source: ValidateChangeForMainBranchOnlyResult, destination: response);
                return response;
            }

            if (createdUser.UserBranches == null ||
                    createdUser.UserBranches.Count() <= 0)
            {
                if (createdUser.MainBranchId <= 0)
                {
                    response.Status = ResponseStatus.ValidationError;

                    TranslationHelper
                    .SetResponseMessages
                        (response, "MustChooseOneOrMoreBranchForTheUser!",
                        "Must Choose One Or More Branch For The User !", lang: userContext.Lang);
                }
                else
                {
                    createdUser.UserBranches = new List<UserBranch>
                    {
                        new UserBranch()
                        {
                            BranchId = createdUser.MainBranchId
                        }
                    };
                }
            }



            try
            {
                unitOfWork.CreateTransaction();


                IdentityResult createUserResponse = await smartUserManagerRepository.CreateAsync(smartUser, createdUser.Password);

                if (!createUserResponse.Succeeded)
                {

                    response.Message = string.Join(",", createUserResponse.Errors.Select(x => x.Description).FirstOrDefault());
                    response.MessageCode = createUserResponse.Errors.Select(x => x.Code).FirstOrDefault();
                    TranslationHelper.SetResponseMessages(response, response.MessageCode, response.Message, lang: userContext.Lang ?? "ar");
                    response.Status = ResponseStatus.ValidationError;


                    unitOfWork.Rollback();
                    return response;
                }
                IdentityResult? addingToRoleResult = new();
                if (createdUser.UserRols == null || createdUser.UserRols.Count == 0)
                {
                    addingToRoleResult = await smartUserManagerRepository.AddToRoleAsync(smartUser, "FullAccess");

                }
                else
                {

                    addingToRoleResult = await smartUserManagerRepository.AddToRolesAsync(smartUser, createdUser.UserRols.ToArray());
                }





                if (!addingToRoleResult.Succeeded)
                {
                    await smartUserManagerRepository.DeleteAsync(smartUser);


                    var message = string.Join(",", createUserResponse.Errors.Select(x => x.Description).ToList().ToArray());

                    TranslationHelper.SetResponseMessages(response, "", message, lang: userContext.Lang);

                    response.Status = ResponseStatus.ValidationError;
                    unitOfWork.Rollback();
                    return response;
                }
                #region User Branches

                var userBranches = createdUser.UserBranches;


                try
                {
                    foreach (var item in userBranches)
                    {
                        item.UserId = smartUser.Id;
                    }
                    var createuserBranchesResponse = userBranchRepository.BulkInsert(userBranches);

                    response.Status = ResponseStatus.Success;
                    unitOfWork.Save();
                }
                catch (Exception ex)
                {
                    unitOfWork.Rollback();
                    TranslationHelper.SetException(response, ex);
                    response.Exception = ex; response.Message = ex.Message;
                    response.Status = ResponseStatus.NotImplemented;
                    return response;
                }

                #endregion


                #region User Groups

                var userGroups = createdUser.UserGroups;


                try
                {
                    foreach (var item in userGroups)
                    {
                        item.UserId = smartUser.Id;
                    }
                    var createuserBranchesResponse = userGroupRepository.BulkInsert(userGroups);

                    response.Status = ResponseStatus.Success;
                    unitOfWork.Save();
                }
                catch (Exception ex)
                {
                    unitOfWork.Rollback();
                    TranslationHelper.SetException(response, ex);
                    response.Exception = ex; response.Message = ex.Message;
                    response.Status = ResponseStatus.NotImplemented;
                    return response;
                }

                #endregion


                unitOfWork.Commit();
                createdUser = mapper.Map<CreatedUser>(smartUser);
                response.Result = createdUser;
                response.Status = ResponseStatus.Success;

            }
            catch (Exception ex)
            {

                unitOfWork.Rollback();
                response.Exception = ex; response.Message = ex.Message;
                response.Status = ResponseStatus.NotImplemented;

            }
            return response;

        }
        public async Task<BaseResponseT<CreatedUser>> Update(CreatedUser updatedUser)
        {
            BaseResponseT<CreatedUser> response = new() { };
            //var getFromDB = smartUserRepository.GetByID(updatedUser.Id);
            //updatedUser.UserName = getFromDB.UserName;
            updatedUser.MainBranchId = userContext.BranchId ?? 0;
            User myuser = mapper.Map<User>(updatedUser);

            UserValidator uservalidate = new(ValidationMode.Update, this, userContext);

            ValidationResult result = await uservalidate.ValidateAsync(myuser);


            if (!result.IsValid)
            {

                response.Status = ResponseStatus.ValidationError;
                response.MessageCode = result.Errors.Select(x => x.ErrorCode).FirstOrDefault();
                response.Message = result.Errors[0].ErrorMessage;
                return response;

            }



            try
            {

                var ValidateChangeForMainBranchOnlyResult = branchValidatorBL.ValidateChangeForMainBranchOnly(userContext, ChangeType.Add);

                if (ValidateChangeForMainBranchOnlyResult.Status != ResponseStatus.Success)
                {
                    TranslationHelper.MapBaseResponse(source: ValidateChangeForMainBranchOnlyResult, destination: response);
                    return response;
                }
                unitOfWork.CreateTransaction();




                User? myuser2 = await smartUserManagerRepository.FindByIdAsync(updatedUser.Id.ToString());



                List<User> getDuplicateUsersList = smartUserRepository.Get(r => r.UserName == myuser.UserName && r.Id != myuser2.Id).ToList();

                if (getDuplicateUsersList.Count() > 0)
                {

                    TranslationHelper.SetResponseMessages(response, "DuplicateUserName", "Duplicate User Name Or Mobile Not Allowed", lang: userContext.Lang ?? "ar");


                    response.Status = ResponseStatus.ValidationError;
                    unitOfWork.Rollback();
                    return response;
                }
                if (updatedUser.UserBranches == null ||
                 updatedUser.UserBranches.Count() <= 0)
                {
                    if (updatedUser.MainBranchId <= 0)
                    {
                        response.Status = ResponseStatus.ValidationError;

                        TranslationHelper
                        .SetResponseMessages
                            (response, "MustChooseOneOrMoreBranchForTheUser!",
                            "Must Choose One Or More Branch For The User !", lang: userContext.Lang);
                    }
                    else
                    {
                        updatedUser.UserBranches = new List<UserBranch>
                    {
                        new UserBranch()
                        {
                            BranchId = updatedUser.MainBranchId,
                            UserId = updatedUser.Id
                        }
                    };
                    }
                }



                myuser2.Email = myuser.Email;
                myuser2.FirstName = myuser.FirstName;
                myuser2.LastName = myuser.LastName;
                myuser2.IsActive = myuser.IsActive;
                myuser2.Gender = myuser.Gender;
                myuser2.BirthDate = myuser.BirthDate;
                myuser2.PhoneNumber = myuser.PhoneNumber;
                myuser2.MobileNumber = myuser.MobileNumber;

                IdentityResult updateUserResponse = await smartUserManagerRepository.UpdateAsync(myuser2);

                if (!updateUserResponse.Succeeded)
                {

                    string message = string.Join(",", updateUserResponse.Errors.Select(x => x.Description).ToList().ToArray());

                    TranslationHelper.SetResponseMessages(response, "", message, lang: userContext.Lang ?? "ar");

                    response.Status = ResponseStatus.ValidationError;
                    unitOfWork.Rollback();
                    return response;
                }
                myuser = await smartUserManagerRepository.FindByIdAsync(updatedUser.Id.ToString());



                /*var roles = await smartUserManagerRepository.GetRolesAsync(myuser);
                await smartUserManagerRepository.RemoveFromRolesAsync(myuser, updatedUser.UserRols);


                var addingToRoleResult = await smartUserManagerRepository.AddToRolesAsync(myuser, updatedUser.UserRols);


                if (!addingToRoleResult.Succeeded)
                {

                    var message = string.Join(",", addingToRoleResult.Errors.Select(x => x.Description).ToList());

                    TranslationHelper.SetValidationMessages(response, "", message, lang: userContext.Lang);

                    response.Status = ResponseStatus.ValidationError;
                    unitOfWork.Rollback();
                    return response;

                }*/


                #region User Branches


                var UserBranches = updatedUser.UserBranches;

                myuser.UserBranches = null;

                var updatedBranches = new List<UserBranch>();
                var DBUserBranches = await userBranchBL.GetByUser(myuser.Id);

                if (DBUserBranches.Result != null)
                {

                    var FindWillDeletedUserBranches = DBUserBranches.Result.Where(dbss => !UserBranches.Any(ss => ss.BranchId == dbss.BranchId))
                        .ToList();

                    foreach (var willDeletedUserBranche in FindWillDeletedUserBranches)
                    {


                        userBranchRepository.Delete(willDeletedUserBranche.Id);

                    }

                    unitOfWork.Save();
                }

                if (UserBranches != null)
                {
                    foreach (var item in updatedUser.UserBranches)
                    {
                        item.UserId = myuser.Id;
                        if (item.Id > 0)
                        {
                            userBranchRepository.Update(item);
                        }
                        else
                        {
                            userBranchRepository.Insert(item);
                        }
                    };

                    unitOfWork.Save();


                }

                #endregion

                #region User Groups


                var UserGroups = updatedUser.UserGroups;

                myuser.UserGroups = null;

                var updatedUserGroups = new List<UserBranch>();
                var DBUserGroups = userGroupRepository.Get(u=>u.UserId == myuser.Id).ToList();

                if (DBUserGroups != null)
                {

                    var FindWillDeletedUserGroups = DBUserGroups.Where(dbss => !UserGroups.Any(ss => ss.GroupId == dbss.GroupId))
                        .ToList();

                    foreach (var willDeletedUserGroup in FindWillDeletedUserGroups)
                    {


                        userGroupRepository.Delete(willDeletedUserGroup.Id);

                    }

                    unitOfWork.Save();
                }

                if (UserGroups != null)
                {
                    foreach (var item in updatedUser.UserGroups)
                    {
                        item.UserId = myuser.Id;
                        if (item.Id > 0)
                        {
                            userGroupRepository.Update(item);
                        }
                        else
                        {
                            userGroupRepository.Insert(item);
                        }
                    };

                    unitOfWork.Save();


                }

                #endregion

                unitOfWork.Commit();

                response.Result = mapper.Map<CreatedUser>(myuser2);
                response.Status = ResponseStatus.Success;

            }
            catch (Exception ex)
            {
                unitOfWork.Rollback();
                TranslationHelper.SetException(response, ex, lang: userContext.Lang ?? "ar");
            }
            return response;
        }
        public async Task<BaseResponseT<bool>> DeleteById(int userId)
        {
            BaseResponseT<bool> response = new();

            var ValidateChangeForMainBranchOnlyResult = branchValidatorBL.ValidateChangeForMainBranchOnly(userContext, ChangeType.Add);

            if (ValidateChangeForMainBranchOnlyResult.Status != ResponseStatus.Success)
            {
                TranslationHelper.MapBaseResponse(source: ValidateChangeForMainBranchOnlyResult, destination: response);
                return response;
            }
            User? myUser = await smartUserRepository.Get(a => a.Id == userId).FirstOrDefaultAsync();
            if (myUser != null)
            {
                unitOfWork.CreateTransaction();
                try
                {
                    var userbranches = userBranchRepository.Get(a => a.UserId == userId).ToList();

                    foreach (var item in userbranches)
                    {
                        userBranchRepository.Delete(item);
                    }

                    var userRoles = smartUserRoleRepository.Get(a => a.UserId == userId).ToList();

                    foreach (var item in userRoles)
                    {
                        smartUserRoleRepository.Delete(item);
                    }


                    smartUserRepository.Delete(myUser);

                    unitOfWork.Save();
                    unitOfWork.Commit();
                }
                catch (Exception ex)
                {
                    unitOfWork.Rollback();
                    response.Status = ResponseStatus.ValidationError;
                    response.Result = false;
                    TranslationHelper.SetResponseMessages(response, "Can'tBeDeletedItIsRelatedToOtherData", "Can't Be Deleted It Is Related To Other Data !", lang: userContext.Lang);
                }
            }

            response.Result = true;
            response.Status = ResponseStatus.Success;
            return response;
        }
        public BaseResponseT<bool> IsEmailUnique(ValidationItems validationItem)
        {
            BaseResponseT<bool> response = new BaseResponseT<bool>();
            try
            {
                //string currentName;
                User duplicateUser = null;
                if (string.IsNullOrEmpty(validationItem.Item))
                {
                    response.Result = true;
                    response.Status = ResponseStatus.Success;
                    return response;
                }
                if (validationItem.validationMode == ValidationMode.Create)
                {
                    duplicateUser = smartUserRepository.Get(x => x.Email.ToLower().Trim() == validationItem.Item.ToLower().Trim()).FirstOrDefault();
                }

                else if (validationItem.validationMode == ValidationMode.Update && validationItem.Id != null)
                {


                    duplicateUser = smartUserRepository.Get(x => x.Email.ToLower() == validationItem.Item.ToLower() && x.Id != validationItem.Id.Value).FirstOrDefault();
                }

                if (duplicateUser == null)
                {
                    response.Result = true;
                }
                else
                {
                    response.Result = false;

                }
                response.Status = ResponseStatus.Success;
            }
            catch (Exception ex)
            {
                response.Result = false;
                response.Status = ResponseStatus.Error;
                response.Exception = ex; response.Message = ex.Message;
            }
            return response;
        }
    }
}

