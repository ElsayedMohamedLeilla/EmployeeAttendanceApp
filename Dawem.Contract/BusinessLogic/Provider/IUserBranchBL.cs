using SmartBusinessERP.Domain.Entities.Provider;
using SmartBusinessERP.Models.Criteria.Others;
using SmartBusinessERP.Models.Response;
using SmartBusinessERP.Models.Response.Others;

namespace SmartBusinessERP.BusinessLogic.Provider.Contract
{
    public interface IUserBranchBL
    {
        Task<GetUserBranchesResponse> GetUserBranches(GetUserBranchCriteria criteria);
        Task<BaseResponseT<List<UserBranch>>> GetByUser(int userId);
        Task<BaseResponseT<List<UserBranch>>> GetByBranch(int branchId);
        BaseResponseT<UserBranch> Create(UserBranch userBranch);
    }
}
