using Dawem.Models.Dtos.Dawem.Permissions.Permissions;

namespace Dawem.Contract.BusinessLogicCore.Dawem
{
    public interface IPermissionBLC
    {
        Task<int> Create(CreatePermissionModel model);
    }
}
