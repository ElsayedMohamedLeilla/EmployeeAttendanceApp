using Dawem.Data;
using Dawem.Domain.Entities.Employees;
using Dawem.Models.Dtos.Provider;

namespace Dawem.Contract.Repository.Lookups
{
    public interface IEmployeeRepository : IGenericRepository<Employee>
    {
        IQueryable<Employee> GetAsQueryable(GetEmployeesCriteria criteria);
    }
}
