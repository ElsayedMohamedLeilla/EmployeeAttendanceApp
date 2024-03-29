using Dawem.Models.Dtos.Dawem.Employees.AssignmentTypes;

namespace Dawem.Contract.BusinessValidation.Dawem.Employees
{
    public interface IAssignmentTypeBLValidation
    {
        Task<bool> CreateValidation(CreateAssignmentTypeModel model);
        Task<bool> UpdateValidation(UpdateAssignmentTypeModel model);
    }
}
