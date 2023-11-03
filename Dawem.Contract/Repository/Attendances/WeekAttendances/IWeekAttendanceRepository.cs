using Dawem.Data;
using Dawem.Domain.Entities.Attendance;
using Dawem.Models.Dtos.Employees.Employees;

namespace Dawem.Contract.Repository.Attendances.WeekAttendances
{
    public interface IWeekAttendanceRepository : IGenericRepository<WeekAttendance>
    {
        IQueryable<WeekAttendance> GetAsQueryable(GetWeekAttendancesCriteria criteria);
    }
}
