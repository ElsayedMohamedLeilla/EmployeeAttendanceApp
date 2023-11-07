using Dawem.Data;
using Dawem.Domain.Entities.Attendance;
using Dawem.Models.Dtos.Employees.Employees;

namespace Dawem.Contract.Repository.Attendances.WeekAttendances
{
    public interface IScheduleRepository : IGenericRepository<Schedule>
    {
        IQueryable<Schedule> GetAsQueryable(GetSchedulesCriteria criteria);
    }
}
