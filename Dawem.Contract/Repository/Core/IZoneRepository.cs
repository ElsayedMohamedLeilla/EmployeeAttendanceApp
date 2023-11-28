using Dawem.Data;
using Dawem.Domain.Entities.Core;
using Dawem.Models.Criteria.Core;

namespace Dawem.Contract.Repository.Core
{
    public interface IZoneRepository : IGenericRepository<Zone>
    {
        IQueryable<Zone> GetAsQueryable(GetZoneCriteria criteria);
    }
}
