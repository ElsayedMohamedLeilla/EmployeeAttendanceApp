using Dawem.Data;
using Dawem.Domain.Entities.Permissions;
using Dawem.Models.Dtos.Permissions.Permissions;

namespace Dawem.Contract.Repository.Permissions
{
    public interface IPermissionScreenRepository : IGenericRepository<PermissionScreen>
    {
        IQueryable<PermissionScreen> GetAsQueryable(GetPermissionScreensCriteria criteria);
    }
}
