using Dawem.Data;
using Dawem.Domain.Entities.Employees;
using Dawem.Models.Dtos.Employees.Employees;
using Dawem.Models.Dtos.Reports.AttendanceSummaryReport;

namespace Dawem.Contract.Repository.Employees
{
    public interface IEmployeeRepository : IGenericRepository<Employee>
    {
        IQueryable<Employee> GetAsQueryable(GetEmployeesCriteria criteria);
        IQueryable<Employee> GetAsQueryableForAttendanceSummary(AttendanceSummaryCritria criteria);
    }
}
