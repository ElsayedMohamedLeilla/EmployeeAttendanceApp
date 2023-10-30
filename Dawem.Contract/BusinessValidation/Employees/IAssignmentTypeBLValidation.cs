using Dawem.Models.Dtos.Employees.AssignmentType;

namespace Dawem.Contract.BusinessValidation.Employees
{
    public interface IAssignmentTypeBLValidation
    {
        Task<bool> CreateValidation(CreateAssignmentTypeModel model);
        Task<bool> UpdateValidation(UpdateAssignmentTypeModel model);
    }
}
