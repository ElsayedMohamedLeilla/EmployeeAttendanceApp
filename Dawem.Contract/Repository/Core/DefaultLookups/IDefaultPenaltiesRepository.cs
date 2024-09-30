using Dawem.Data;
using Dawem.Domain.Entities.Core.DefaultLookus;
using Dawem.Models.Criteria.DefaultLookups;

namespace Dawem.Contract.Repository.Core.DefaultLookups
{
    public interface IDefaultPenaltiesRepository : IGenericRepository<DefaultLookup>
    {
        IQueryable<DefaultLookup> GetAsQueryable(GetDefaultPenaltiesCriteria criteria);
    }
}
