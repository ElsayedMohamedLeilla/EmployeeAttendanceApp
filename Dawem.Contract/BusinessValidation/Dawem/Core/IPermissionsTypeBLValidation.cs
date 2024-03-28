using Dawem.Models.Dtos.Dawem.Core.PermissionsTypes;

namespace Dawem.Contract.BusinessValidation.Dawem.Core
{
    public interface IPermissionsTypeBLValidation
    {
        Task<bool> CreateValidation(CreatePermissionTypeDTO model);
        Task<bool> UpdateValidation(UpdatePermissionTypeDTO model);
    }
}
