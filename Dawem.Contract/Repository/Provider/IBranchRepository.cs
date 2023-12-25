using Dawem.Data;
using Dawem.Domain.Entities.Providers;
using Dawem.Models.Criteria.Providers;
using Dawem.Models.Dtos.Identities;
using Dawem.Translations;

namespace Dawem.Contract.Repository.Provider
{
    public interface IBranchRepository : IGenericRepository<Branch>
    {
        IQueryable<Branch> GetAsQueryable(GetBranchesCriteria criteria, string includeProperties = LeillaKeys.EmptyString, UserDTO user = null);
    }
}
