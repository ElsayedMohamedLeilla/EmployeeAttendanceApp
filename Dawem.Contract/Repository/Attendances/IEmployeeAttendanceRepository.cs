using Dawem.Data;
using Dawem.Domain.Entities.Attendances;
using Dawem.Models.Dtos.Attendances;
using Dawem.Models.Dtos.Employees.Department;

namespace Dawem.Contract.Repository.Attendances
{
    public interface IEmployeeAttendanceRepository : IGenericRepository<EmployeeAttendance>
    {
        IQueryable<EmployeeAttendance> GetAsQueryable(GetEmployeeAttendancesForWebAdminCriteria criteria);
        IQueryable<EmployeeAttendance> GetForStatusAsQueryable(GetStatusBaseModel model);
    }
}
