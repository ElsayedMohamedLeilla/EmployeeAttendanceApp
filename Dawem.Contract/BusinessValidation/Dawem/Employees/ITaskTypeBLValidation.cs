using Dawem.Models.Dtos.Dawem.Employees.TaskTypes;

namespace Dawem.Contract.BusinessValidation.Dawem.Employees
{
    public interface ITaskTypeBLValidation
    {
        Task<bool> CreateValidation(CreateTaskTypeModel model);
        Task<bool> UpdateValidation(UpdateTaskTypeModel model);
    }
}
