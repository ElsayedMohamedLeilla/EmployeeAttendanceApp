using Dawem.Models.Dtos.Employees.TaskType;

namespace Dawem.Contract.BusinessValidation.Employees
{
    public interface ITaskTypeBLValidation
    {
        Task<bool> CreateValidation(CreateTaskTypeModel model);
        Task<bool> UpdateValidation(UpdateTaskTypeModel model);
    }
}
