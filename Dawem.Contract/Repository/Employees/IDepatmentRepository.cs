using Dawem.Data;
using Dawem.Domain.Entities.Employees;
using Dawem.Models.Dtos.Dawem.Employees.Departments;

namespace Dawem.Contract.Repository.Employees
{
    public interface IDepartmentRepository : IGenericRepository<Department>
    {
        IQueryable<Department> GetAsQueryable(GetDepartmentsCriteria criteria);
    }
}
