using Dawem.Data;
using Dawem.Domain.Entities.UserManagement;
using Dawem.Models.Criteria.Core;

namespace Dawem.Contract.Repository.Core
{
    public interface IRoleRepository : IGenericRepository<Role>
    {
        IQueryable<Role> GetAsQueryable(GetRoleCriteria criteria);

    }
}
