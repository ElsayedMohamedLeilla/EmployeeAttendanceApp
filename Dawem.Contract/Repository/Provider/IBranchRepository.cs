using Dawem.Data;
using Dawem.Domain.Entities.Provider;
using Dawem.Models.Criteria.Provider;
using Dawem.Models.Dtos.Identity;
using Dawem.Translations;

namespace Dawem.Contract.Repository.Provider
{
    public interface IBranchRepository : IGenericRepository<Branch>
    {
        IQueryable<Branch> GetAsQueryable(GetBranchesCriteria criteria, string includeProperties = DawemKeys.EmptyString, UserDTO user = null);
    }
}
