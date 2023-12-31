using Dawem.Data;
using Dawem.Domain.Entities.Permissions;
using Dawem.Models.Criteria.Others;
using Dawem.Translations;

namespace Dawem.Contract.Repository.Permissions
{
    public interface IPermissionLogRepository : IGenericRepository<PermissionLog>
    {
        IQueryable<PermissionLog> GetAsQueryable(GetPermissionLogsCriteria criteria, string includeProperties = LeillaKeys.EmptyString);
    }
}
