using Dawem.Data;
using Dawem.Domain.Entities.Employees;
using Dawem.Models.Dtos.Provider;

namespace Dawem.Contract.Repository.Lookups
{
    public interface IDepartmentRepository : IGenericRepository<Department>
    {
        IQueryable<Department> GetAsQueryable(GetDepartmentsCriteria criteria);
    }
}
