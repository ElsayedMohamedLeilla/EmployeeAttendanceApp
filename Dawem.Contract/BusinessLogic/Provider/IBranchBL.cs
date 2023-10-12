using Dawem.Models.Dtos.Provider;
using Dawem.Models.Response;
using Dawem.Models.Response.Core;
using Dawem.Models.Response.Provider;
using SmartBusinessERP.Models.Criteria.Core;
using SmartBusinessERP.Models.Criteria.Provider;

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
