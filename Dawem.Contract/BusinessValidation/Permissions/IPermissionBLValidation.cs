using Dawem.Models.Dtos.Employees.AssignmentTypes;

namespace Dawem.Contract.BusinessValidation.Employees
{
    public interface IPermissionBLValidation
    {
        Task<bool> CreateValidation(CreatePermissionModel model);
        Task<bool> UpdateValidation(UpdatePermissionModel model);
    }
}
