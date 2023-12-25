using Dawem.Models.Dtos.Core.PermissionsTypes;

namespace Dawem.Contract.BusinessValidation.Core
{
    public interface IPermissionsTypeBLValidation
    {
        Task<bool> CreateValidation(CreatePermissionTypeDTO model);
        Task<bool> UpdateValidation(UpdatePermissionTypeDTO model);
    }
}
