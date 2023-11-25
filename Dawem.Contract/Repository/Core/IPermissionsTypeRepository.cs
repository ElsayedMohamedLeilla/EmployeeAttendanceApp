using Dawem.Data;
using Dawem.Domain.Entities.Core;
using Dawem.Models.Criteria.Core;

namespace Dawem.Contract.Repository.Core
{
    public interface IPermissionsTypeRepository : IGenericRepository<PermissionType>
    {
        IQueryable<PermissionType> GetAsQueryable(GetPermissionsTypesCriteria criteria);
    }
}
