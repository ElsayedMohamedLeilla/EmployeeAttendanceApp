using SmartBusinessERP.Models.Criteria.Core;
using SmartBusinessERP.Models.Criteria.Provider;
using SmartBusinessERP.Models.Dtos.Provider;
using SmartBusinessERP.Models.Response;
using SmartBusinessERP.Models.Response.Core;
using SmartBusinessERP.Models.Response.Provider;

namespace SmartBusinessERP.BusinessLogic.Provider.Contract
{
    public interface IBranchBL
    {
        Task<BaseResponseT<int>> Create(BranchDTO branch);
        Task<GetBranchInfoResponse> GetInfo(GetBranchInfoCriteria criteria);
        Task<GetBranchesResponse> Get(GetBranchesCriteria criteria);
        Task<BaseResponseT<bool>> Update(BranchDTO branch);
        Task<BaseResponseT<bool>> Delete(int Id);
    }
}
