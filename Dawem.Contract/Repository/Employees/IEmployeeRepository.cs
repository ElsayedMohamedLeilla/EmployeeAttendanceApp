using Dawem.Data;
using Dawem.Domain.Entities.Employees;
using Dawem.Models.Dtos.Dawem.Employees.Employees;
using Dawem.Models.Dtos.Dawem.Reports.AttendanceSummaryReport;
using Dawem.Models.DTOs.Dawem.Employees.Employees;

namespace Dawem.Contract.Repository.Employees
{
    public interface IEmployeeRepository : IGenericRepository<Employee>
    {
        IQueryable<Employee> GetAsQueryable(GetEmployeesCriteria criteria);
        IQueryable<Employee> GetAsQueryableForAttendanceSummary(AttendanceSummaryCritria criteria);
        IQueryable<Employee> GetAsQueryableForEmployeeSchedulePlan(GetEmployeeSchedulePlanCritria criteria);

        
    }
}
