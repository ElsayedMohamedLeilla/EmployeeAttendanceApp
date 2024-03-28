using Dawem.Data;
using Dawem.Domain.Entities.Permissions;
using Dawem.Models.Dtos.Dawem.Permissions.Permissions;

namespace Dawem.Contract.Repository.Permissions
{
    public interface IPermissionRepository : IGenericRepository<Permission>
    {
        IQueryable<Permission> GetAsQueryable(GetPermissionsCriteria criteria);
    }
}
