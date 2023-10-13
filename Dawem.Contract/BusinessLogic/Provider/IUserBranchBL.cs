using Dawem.Domain.Entities.Provider;
using Dawem.Models.Criteria.Others;
using Dawem.Models.Response;
using Dawem.Models.Response.Others;

namespace Dawem.Contract.BusinessLogic.Provider
{
    public interface IUserBranchBL
    {
        Task<GetUserBranchesResponse> GetUserBranches(GetUserBranchCriteria criteria);
        Task<BaseResponseT<List<UserBranch>>> GetByUser(int userId);
        Task<BaseResponseT<List<UserBranch>>> GetByBranch(int branchId);
        BaseResponseT<UserBranch> Create(UserBranch userBranch);
    }
}
