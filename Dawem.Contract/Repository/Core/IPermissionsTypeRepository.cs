using Dawem.Data;
using Dawem.Domain.Entities.Core;
using Dawem.Models.Criteria.Core;

namespace Dawem.Contract.Repository.Core
{
    public interface IPermissionsTypeRepository : IGenericRepository<PermissionsType>
    {
        IQueryable<PermissionsType> GetAsQueryable(GetPermissionsTypesCriteria criteria);
    }
}
