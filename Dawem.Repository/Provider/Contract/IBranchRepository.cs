using Dawem.Data;
using Dawem.Domain.Entities.Provider;

namespace Dawem.Repository.Provider.Contract
{
    public interface IBranchRepository : IGenericRepository<Branch>
    {
        //IQueryable<Branch> GetAsQueryable(GetBranchesCriteria criteria, string includeProperties = "", SmartUserDTO user = null);
    }
}
