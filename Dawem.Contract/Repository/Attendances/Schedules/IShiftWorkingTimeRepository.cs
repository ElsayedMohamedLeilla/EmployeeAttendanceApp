using Dawem.Data;
using Dawem.Domain.Entities.Attendance;
using Dawem.Models.Dtos.Employees.Attendance.ShiftWorkingTimes;

namespace Dawem.Contract.Repository.Attendances.Schedules
{
    public interface IShiftWorkingTimeRepository : IGenericRepository<ShiftWorkingTime>
    {
        IQueryable<ShiftWorkingTime> GetAsQueryable(GetShiftWorkingTimesCriteria criteria);
    }
}
