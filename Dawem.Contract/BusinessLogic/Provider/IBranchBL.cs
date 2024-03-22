using Dawem.Models.Criteria.Core;
using Dawem.Models.Criteria.Providers;
using Dawem.Models.Dtos.Providers;
using Dawem.Models.ResponseModels;

namespace Dawem.Contract.BusinessLogic.Provider
{
    public interface IBranchBL
    {
        Task<int> Create(BranchDTO branch);
        Task<BranchDTO> GetInfo(GetBranchInfoCriteria criteria);
        Task<GetBranchesResponseModel> Get(GetCompanyBranchesCriteria criteria);
        Task<bool> Update(BranchDTO branch);
        Task<bool> Delete(int Id);
    }
}
