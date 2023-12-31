using Dawem.Data;
using Dawem.Domain.Entities.Permissions;
using Dawem.Models.Dtos.Permissions.PermissionLogs;

namespace Dawem.Contract.Repository.Permissions
{
    public interface IPermissionLogRepository : IGenericRepository<PermissionLog>
    {
        IQueryable<PermissionLog> GetAsQueryable(GetPermissionLogsCriteria criteria);
    }
}
