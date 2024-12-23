using Dawem.Data;
using Dawem.Domain.Entities.Core;
using Dawem.Models.Criteria.Core;

namespace Dawem.Contract.Repository.Core
{
    public interface IOvertimeTypeRepository : IGenericRepository<OvertimeType>
    {
        IQueryable<OvertimeType> GetAsQueryable(GetOvertimeTypesCriteria criteria);
    }
}
