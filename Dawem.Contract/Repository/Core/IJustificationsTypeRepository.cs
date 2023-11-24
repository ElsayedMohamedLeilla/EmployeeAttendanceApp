using Dawem.Data;
using Dawem.Domain.Entities.Core;
using Dawem.Models.Criteria.Core;

namespace Dawem.Contract.Repository.Core
{
    public interface IJustificationsTypeRepository : IGenericRepository<JustificationType>
    {
        IQueryable<JustificationType> GetAsQueryable(GetJustificationsTypesCriteria criteria);
    }
}
