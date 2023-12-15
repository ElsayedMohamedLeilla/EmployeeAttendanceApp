using Dawem.Data;
using Dawem.Domain.Entities.Core;
using Dawem.Models.Criteria.Core;

namespace Dawem.Contract.Repository.Core
{
    public interface IHolidayRepository : IGenericRepository<Holiday>
    {
        IQueryable<Holiday> GetAsQueryable(GetHolidayCriteria criteria);
    }
}
