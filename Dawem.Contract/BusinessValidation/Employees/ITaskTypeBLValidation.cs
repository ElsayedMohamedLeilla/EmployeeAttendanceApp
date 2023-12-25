using Dawem.Models.Dtos.Employees.TaskTypes;

namespace Dawem.Contract.BusinessValidation.Employees
{
    public interface ITaskTypeBLValidation
    {
        Task<bool> CreateValidation(CreateTaskTypeModel model);
        Task<bool> UpdateValidation(UpdateTaskTypeModel model);
    }
}
