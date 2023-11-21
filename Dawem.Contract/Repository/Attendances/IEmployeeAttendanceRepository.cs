using Dawem.Data;
using Dawem.Domain.Entities.Employees;
using Dawem.Models.Dtos.Employees.HolidayType;

namespace Dawem.Contract.Repository.Employees
{
    public interface IEmployeeAttendanceRepository : IGenericRepository<EmployeeAttendance>
    {
        IQueryable<EmployeeAttendance> GetAsQueryable(GetEmployeeAttendancesCriteria criteria);
    }
}
