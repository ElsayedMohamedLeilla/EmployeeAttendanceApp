using Dawem.Domain.Entities.Providers;

namespace Dawem.Contract.BusinessLogic.Dawem.Provider
{
    public interface IUserBranchBL
    {
        Task<List<UserBranch>> GetByUser(int userId);
        Task<List<UserBranch>> GetByBranch(int branchId);
        Task<int> Create(UserBranch userBranch);
    }
}
