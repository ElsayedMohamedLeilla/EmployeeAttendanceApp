using Dawem.Data;
using Dawem.Domain.Entities.Attendance;
using Dawem.Models.Dtos.Attendances.Schedules;

namespace Dawem.Contract.Repository.Attendances.Schedules
{
    public interface IScheduleRepository : IGenericRepository<Schedule>
    {
        IQueryable<Schedule> GetAsQueryable(GetSchedulesCriteria criteria);
    }
}
