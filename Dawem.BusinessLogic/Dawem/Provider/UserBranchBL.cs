using Dawem.Contract.BusinessLogic.Dawem.Provider;
using Dawem.Contract.Repository.Manager;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Providers;
using Dawem.Models.Context;
using Dawem.Models.DTOs.Dawem.Generic.Exceptions;
using Dawem.Repository.UserManagement;
using Dawem.Translations;
using Microsoft.EntityFrameworkCore;

namespace Dawem.BusinessLogic.Dawem.Provider
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
        public async Task<List<UserBranch>> GetByUser(int userId)
        {
            var userBranch = new List<UserBranch>();

            var userBranches = await repositoryManager.UserBranchRepository
                .Get(user => user.UserId == userId)
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
                .Get(user => user.BranchId == branchId)
                .ToListAsync();

            if (userBranches == null || userBranches.Count() <= 0)
            {
                throw new BusinessValidationException(LeillaKeys.SorryThereIsNoBranchesForThisUser);
            }

            return userBranches;
        }


    }
}
