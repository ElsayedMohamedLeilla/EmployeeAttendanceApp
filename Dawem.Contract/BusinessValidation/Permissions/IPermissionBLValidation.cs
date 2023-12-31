using Dawem.Models.Dtos.Permissions.Permissions;

namespace Dawem.Contract.BusinessValidation.Permissions
{
    public interface IPermissionBLValidation
    {
        Task<bool> CreateValidation(CreatePermissionModel model);
        Task<bool> UpdateValidation(UpdatePermissionModel model);
    }
}
