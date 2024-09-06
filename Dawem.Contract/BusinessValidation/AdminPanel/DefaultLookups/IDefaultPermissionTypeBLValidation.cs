using Dawem.Models.Dtos.AdminPanel.DefaultLookups.DefaultPermissionsTypes;

namespace Dawem.Contract.BusinessValidation.AdminPanel.DefaultLookups
{
    public interface IDefaultPermissionTypeBLValidation
    {
        Task<bool> CreateValidation(CreateDefaultPermissionsTypeDTO model);
        Task<bool> UpdateValidation(UpdateDefaultPermissionsTypeDTO model);
    }
}
