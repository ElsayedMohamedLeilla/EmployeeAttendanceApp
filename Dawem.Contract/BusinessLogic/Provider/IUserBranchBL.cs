using Dawem.Domain.Entities.Provider;
using Dawem.Models.Criteria.Others;
using Dawem.Models.ResponseModels;

namespace Dawem.Contract.BusinessLogic.Provider
{
    public interface IUserBranchBL
    {
        Task<GetUserBranchesResponseModel> GetUserBranches(GetUserBranchCriteria criteria);
        Task<List<UserBranch>> GetByUser(int userId);
        Task<List<UserBranch>> GetByBranch(int branchId);
        Task<int> Create(UserBranch userBranch);
    }
}
