using Dawem.Data;
using Dawem.Domain.Entities.Attendances;
using Dawem.Models.Dtos.Attendances;

namespace Dawem.Contract.Repository.Attendances
{
    public interface IEmployeeAttendanceRepository : IGenericRepository<EmployeeAttendance>
    {
        IQueryable<EmployeeAttendance> GetAsQueryable(GetEmployeeAttendancesForWebAdminCriteria criteria);
    }
}
