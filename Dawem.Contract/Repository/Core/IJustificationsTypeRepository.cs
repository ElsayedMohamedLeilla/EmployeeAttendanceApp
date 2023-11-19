using Dawem.Data;
using Dawem.Domain.Entities.Core;
using Dawem.Models.Criteria.Core;

namespace Dawem.Contract.Repository.Core
{
    public interface IJustificationsTypeRepository : IGenericRepository<JustificationsType>
    {
        IQueryable<JustificationsType> GetAsQueryable(GetJustificationsTypesCriteria criteria);
    }
}
