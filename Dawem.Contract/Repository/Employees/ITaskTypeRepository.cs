using Dawem.Data;
using Dawem.Domain.Entities.Employees;
using Dawem.Models.Dtos.Employees.TaskTypes;

namespace Dawem.Contract.Repository.Employees
{
    public interface ITaskTypeRepository : IGenericRepository<TaskType>
    {
        IQueryable<TaskType> GetAsQueryable(GetTaskTypesCriteria criteria);
    }
}
