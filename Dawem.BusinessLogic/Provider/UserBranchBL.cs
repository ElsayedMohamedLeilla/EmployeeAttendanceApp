using Dawem.Contract.BusinessLogic.Provider;
using Dawem.Models.Criteria.Others;
using Microsoft.EntityFrameworkCore;
using SmartBusinessERP.Areas.Identity.Data.UserManagement;
using SmartBusinessERP.Data;
using SmartBusinessERP.Data.UnitOfWork;
using SmartBusinessERP.Domain.Entities.Provider;
using SmartBusinessERP.Enums;
using SmartBusinessERP.Helpers;
using SmartBusinessERP.Models.Context;
using SmartBusinessERP.Models.Dtos.Provider;
using SmartBusinessERP.Models.Response;
using SmartBusinessERP.Models.Response.Others;
using SmartBusinessERP.Repository.Provider.Contract;
using SmartBusinessERP.Repository.UserManagement;

namespace Dawem.BusinessLogic.Provider
{
    public class UserBranchBL : IUserBranchBL
    {

        private readonly IUserBranchRepository userBranchRepository;
        private readonly RequestHeaderContext userContext;
        private IUnitOfWork<ApplicationDBContext> unitOfWork;
        private readonly SmartUserManagerRepository smartUserManagerRepository;


        public UserBranchBL(IUnitOfWork<ApplicationDBContext> _unitOfWork, RequestHeaderContext _userContext, IUserBranchRepository _userBranchRepository, SmartUserManagerRepository _smartUserManagerRepository)
        {

            userContext = _userContext;

            userBranchRepository = _userBranchRepository;
            smartUserManagerRepository = _smartUserManagerRepository;
            unitOfWork = _unitOfWork;


        }
        public BaseResponseT<UserBranch> Create(UserBranch userBranch)
        {
            BaseResponseT<UserBranch> response = new BaseResponseT<UserBranch>();
            try
            {
                response.Result = userBranchRepository.Insert(userBranch);
                unitOfWork.Save();
                response.Status = ResponseStatus.Success;
            }
            catch (Exception ex)
            {
                TranslationHelper.SetException(response, ex);
            }
            return response;
        }
        public async Task<GetUserBranchesResponse> GetUserBranches(GetUserBranchCriteria criteria)
        {

            GetUserBranchesResponse response = new()
            {
                Status = ResponseStatus.Success
            };
            try
            {

                SmartUser user = new();

                #region Get User Using AccessToken
                var Email = criteria.UserName;
                if (!string.IsNullOrEmpty(Email))
                {
                    user = await smartUserManagerRepository.FindByEmailAsync(Email);
                    if (user == null)
                    {
                        TranslationHelper.SetSearchResultMessages(response, "UserNameNotExist", "Email Not Exist", lang: userContext.Lang ?? "ar");
                        response.Status = ResponseStatus.ValidationError;
                        return response;
                    }
                }

                #endregion

                #region Handle User Role

                var roles = await smartUserManagerRepository.GetRolesAsync(user);


                if (roles.FirstOrDefault(r => r == "FullAccess") == null)
                {
                    var addingToRoleResult = await smartUserManagerRepository.AddToRoleAsync(user, "FullAccess");

                    if (addingToRoleResult.Succeeded)
                    {
                        roles.Add("FullAccess");
                    }
                }



                #endregion

                #region Check Password



                bool checkPasswordAsyncRes = await smartUserManagerRepository.CheckPasswordAsync(user, criteria.Password);
                if (!checkPasswordAsyncRes)
                {
                    TranslationHelper.SetSearchResultMessages(response, "PasswordIncorrect", "Sorry! Password Incorrect. Enter Correct Password For Selected User !", userContext.Lang ?? "ar");

                    response.Status = ResponseStatus.ValidationError;
                    return response;
                }


                #endregion

                #region paging

                int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
                int take = PagingHelper.Take(criteria.PageSize);

                var query = userBranchRepository.Get(a => a.UserId == user.Id, IncludeProperties: "Branch");



                var queryPaged = criteria.PagingEnabled ? query.Skip(skip).Take(take) : query;

                #endregion

                var branches = queryPaged.Select(c => new BranchLiteDTO()
                {
                    Id = c.BranchId,
                    GlobalName = c.Branch.BranchName
                }).ToList();


                response.UserBranches = branches;
                response.TotalCount = query.ToList().Count();
                response.Status = ResponseStatus.Success;
            }
            catch (Exception ex)
            {
                response.Exception = ex; response.Message = ex.Message;
                response.Status = ResponseStatus.Error;
            }

            return response;

        }


        public async Task<BaseResponseT<List<UserBranch>>> GetByUser(int userId)
        {
            BaseResponseT<List<UserBranch>> response = new BaseResponseT<List<UserBranch>>();
            try
            {
                var userBranches = await userBranchRepository
                    .Get(user => user.UserId == userId, IncludeProperties: "Branch").ToListAsync();

                if (userBranches != null && userBranches.Count() > 0)
                {
                    response.Result = userBranches;
                    response.Status = ResponseStatus.Success;
                }
                else
                {
                    response.Result = null;
                    response.Status = ResponseStatus.ValidationError;
                    TranslationHelper.SetValidationMessages(response, "NoBranchForThisUser", "No Branch For This User !");

                }
            }
            catch (Exception ex)
            {
                response.Result = null;
                response.Status = ResponseStatus.Error;
                response.Exception = ex; response.Message = ex.Message;
            }
            return response;
        }

        public async Task<BaseResponseT<List<UserBranch>>> GetByBranch(int branchId)
        {
            BaseResponseT<List<UserBranch>> response = new BaseResponseT<List<UserBranch>>();
            try
            {
                var userBranches = await userBranchRepository
                    .GetWithTracking(user => user.BranchId == branchId, IncludeProperties: "Branch").ToListAsync();

                if (userBranches != null && userBranches.Count() > 0)
                {
                    response.Result = userBranches;
                    response.Status = ResponseStatus.Success;
                }
                else
                {
                    response.Result = null;
                    response.Status = ResponseStatus.ValidationError;
                    TranslationHelper.SetValidationMessages(response, "NoBranchForThisUser", "No Branch For This User !");

                }
            }
            catch (Exception ex)
            {
                response.Result = null;
                response.Status = ResponseStatus.Error;
                response.Exception = ex; response.Message = ex.Message;
            }
            return response;
        }

    }
}
