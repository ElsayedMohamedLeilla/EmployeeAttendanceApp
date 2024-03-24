using Dawem.Data;
using Dawem.Domain.Entities.Providers;
using Dawem.Models.Criteria.Providers;

namespace Dawem.Contract.Repository.Provider
{
    public interface ICompanyBranchRepository : IGenericRepository<CompanyBranch>
    {
        IQueryable<CompanyBranch> GetAsQueryable(GetCompanyBranchesCriteria criteria);
    }
}
