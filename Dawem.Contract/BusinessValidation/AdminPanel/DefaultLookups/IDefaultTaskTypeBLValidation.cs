using Dawem.Models.Dtos.AdminPanel.DefaultLookups.DefaultTasksTypes;

namespace Dawem.Contract.BusinessValidation.AdminPanel.DefaultLookups
{
    public interface IDefaultTaskTypeBLValidation
    {
        Task<bool> CreateValidation(CreateDefaultTasksTypeDTO model);
        Task<bool> UpdateValidation(UpdateDefaultTasksTypeDTO model);
    }
}
