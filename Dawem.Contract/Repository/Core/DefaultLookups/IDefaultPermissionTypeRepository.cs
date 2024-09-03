using Dawem.Data;
using Dawem.Domain.Entities.Core.DefaultLookus;
using Dawem.Models.Criteria.DefaultLookups;

namespace Dawem.Contract.Repository.Core.DefaultLookups
{
    public interface IDefaultPermissionTypeRepository : IGenericRepository<DefaultLookup>
    {
        IQueryable<DefaultLookup> GetAsQueryable(GetDefaultPermissionTypeCriteria criteria);
    }
}
