using Dawem.Models.Dtos.Dawem.Permissions.Permissions;

namespace Dawem.Contract.BusinessLogic.Dawem.Permissions
{
    public interface IPermissionBLC
    {
        Task<int> Create(CreatePermissionModel model);
    }
}
