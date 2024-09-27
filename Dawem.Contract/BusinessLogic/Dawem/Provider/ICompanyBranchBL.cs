using Dawem.Models.Criteria.Providers;
using Dawem.Models.Response.Dawem.Providers.CompanyBranches;

namespace Dawem.Contract.BusinessLogic.Dawem.Provider
{
    public interface ICompanyBranchBL
    {
        Task<GetCompanyBranchByIdResponseModel> GetById(int branchId);
        Task<GetCompanyBranchesForDropDownResponse> GetForDropDown(GetCompanyBranchesCriteria model);
    }
}
