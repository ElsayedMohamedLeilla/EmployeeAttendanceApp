using Dawem.Data;
using Dawem.Domain.Entities.Employees;
using Dawem.Models.Dtos.Employees.Employees;

namespace Dawem.Contract.Repository.Employees
{
    public interface IEmployeeRepository : IGenericRepository<Employee>
    {
        IQueryable<Employee> GetAsQueryable(GetEmployeesCriteria criteria);
    }
}
