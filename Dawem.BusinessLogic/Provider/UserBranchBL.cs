using Dawem.Contract.BusinessLogic.Provider;
using Dawem.Contract.Repository.Manager;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Providers;
using Dawem.Domain.Entities.UserManagement;
using Dawem.Helpers;
using Dawem.Models.Context;
using Dawem.Models.Criteria.Others;
using Dawem.Models.Dtos.Providers;
using Dawem.Models.Exceptions;
using Dawem.Models.ResponseModels;
using Dawem.Repository.UserManagement;
using Dawem.Translations;
using Microsoft.EntityFrameworkCore;

namespace Dawem.BusinessLogic.Provider
{
    public class UserBranchBL : IUserBranchBL
    {

        private readonly RequestInfo requestHeaderContext;
        private IUnitOfWork<ApplicationDBContext> unitOfWork;
        private readonly UserManagerRepository userManagerRepository;
        private readonly IRepositoryManager repositoryManager;


        public UserBranchBL(IUnitOfWork<ApplicationDBContext> _unitOfWork, RequestInfo _requestHeaderContext,
            IRepositoryManager _repositoryManager, UserManagerRepository _userManagerRepository)
        {
            requestHeaderContext = _requestHeaderContext;
            repositoryManager = _repositoryManager;
            userManagerRepository = _userManagerRepository;
            unitOfWork = _unitOfWork;


        }
        public async Task<int> Create(UserBranch userBranch)
        {
            repositoryManager.UserBranchRepository.Insert(userBranch);
            await unitOfWork.SaveAsync();
            return userBranch.Id;
        }
        public async Task<GetUserBranchesResponseModel> GetUserBranches(GetUserBranchCriteria criteria)
        {
            #region Get User Using AccessToken

            MyUser user = new();

            var Email = criteria.UserName;
            if (!string.IsNullOrEmpty(Email))
            {
                user = await userManagerRepository.FindByEmailAsync(Email);
                if (user == null)
                {
                    throw new BusinessValidationException(LeillaKeys.SorryUserNotFound);
                }
            }

            #endregion

            #region Handle User Role

            var roles = await userManagerRepository.GetRolesAsync(user);

            if (roles.FirstOrDefault(r => r == LeillaKeys.RoleFULLACCESS) == null)
            {
                var addingToRoleResult = await userManagerRepository.AddToRoleAsync(user, LeillaKeys.RoleFULLACCESS);

                if (addingToRoleResult.Succeeded)
                {
                    roles.Add(LeillaKeys.RoleFULLACCESS);
                }
            }

            #endregion

            #region Check Password

            bool checkPasswordAsyncRes = await userManagerRepository.CheckPasswordAsync(user, criteria.Password);
            if (!checkPasswordAsyncRes)
            {
                throw new BusinessValidationException(LeillaKeys.SorryPasswordIncorrectEnterCorrectPasswordForSelectedUser);
            }


            #endregion

            #region paging

            int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            int take = PagingHelper.Take(criteria.PageSize);

            var query = repositoryManager.UserBranchRepository.Get(a => a.UserId == user.Id, IncludeProperties: nameof(UserBranch.Branch));
            var queryPaged = criteria.PagingEnabled ? query.Skip(skip).Take(take) : query;

            #endregion

            var userBranches = queryPaged.Select(c => new BranchLiteDTO()
            {
                Id = c.BranchId,
                GlobalName = c.Branch.Name
            }).ToList();

            return new GetUserBranchesResponseModel
            {
                UserBranches = userBranches,
                TotalCount = await query.CountAsync()
            };
        }


        public async Task<List<UserBranch>> GetByUser(int userId)
        {
            var userBranch = new List<UserBranch>();

            var userBranches = await repositoryManager.UserBranchRepository
                .Get(user => user.UserId == userId, IncludeProperties: nameof(UserBranch.Branch))
                .ToListAsync();

            if (userBranches == null || userBranches.Count() <= 0)
            {
                throw new BusinessValidationException(LeillaKeys.SorryThereIsNoBranchesForThisUser);
            }

            return userBranches;
        }

        public async Task<List<UserBranch>> GetByBranch(int branchId)
        {
            var userBranch = new List<UserBranch>();

            var userBranches = await repositoryManager.UserBranchRepository
                .Get(user => user.BranchId == branchId, IncludeProperties: nameof(UserBranch.Branch))
                .ToListAsync();

            if (userBranches == null || userBranches.Count() <= 0)
            {
                throw new BusinessValidationException(LeillaKeys.SorryThereIsNoBranchesForThisUser);
            }

            return userBranches;
        }


    }
}
