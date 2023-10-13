using Dawem.Models.Criteria.Core;
using Dawem.Models.Criteria.Provider;
using Dawem.Models.Dtos.Provider;
using Dawem.Models.Response;
using Dawem.Models.Response.Core;
using Dawem.Models.Response.Provider;
using Dawem.Models.ResponseModels;

namespace Dawem.Contract.BusinessLogic.Provider
{
    public interface IBranchBL
    {
        Task<BaseResponseT<int>> Create(BranchDTO branch);
        Task<GetBranchInfoResponse> GetInfo(GetBranchInfoCriteria criteria);
        Task<GetBranchesResponseModel> Get(GetBranchesCriteria criteria);
        Task<BaseResponseT<bool>> Update(BranchDTO branch);
        Task<BaseResponseT<bool>> Delete(int Id);
    }
}
