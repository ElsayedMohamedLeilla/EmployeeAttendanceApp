using Dawem.Data;
using Dawem.Domain.Entities.Schedules;
using Dawem.Models.Dtos.Dawem.Schedules.Schedules;

namespace Dawem.Contract.Repository.Schedules.Schedules
{
    public interface IScheduleRepository : IGenericRepository<Schedule>
    {
        IQueryable<Schedule> GetAsQueryable(GetSchedulesCriteria criteria);
    }
}
