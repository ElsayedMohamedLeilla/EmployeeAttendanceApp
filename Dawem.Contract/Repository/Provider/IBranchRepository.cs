using Dawem.Data;
using Dawem.Domain.Entities.Provider;

namespace Dawem.Contract.Repository.Provider
{
    public interface IBranchRepository : IGenericRepository<Branch>
    {
        //IQueryable<Branch> GetAsQueryable(GetBranchesCriteria criteria, string includeProperties = DawemKeys.EmptyString, SmartUserDTO user = null);
    }
}
