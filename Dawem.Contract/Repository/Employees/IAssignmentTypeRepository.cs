using Dawem.Data;
using Dawem.Domain.Entities.Employees;
using Dawem.Models.Dtos.Employees.AssignmentType;

namespace Dawem.Contract.Repository.Employees
{
    public interface IAssignmentTypeRepository : IGenericRepository<AssignmentType>
    {
        IQueryable<AssignmentType> GetAsQueryable(GetAssignmentTypesCriteria criteria);
    }
}
