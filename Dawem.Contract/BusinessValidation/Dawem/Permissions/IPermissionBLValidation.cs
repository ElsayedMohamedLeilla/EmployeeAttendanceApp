using Dawem.Models.Dtos.Dawem.Permissions.Permissions;

namespace Dawem.Contract.BusinessValidation.Dawem.Permissions
{
    public interface IPermissionBLValidation
    {
        Task<bool> CreateValidation(CreatePermissionModel model);
        Task<bool> UpdateValidation(UpdatePermissionModel model);
    }
}
