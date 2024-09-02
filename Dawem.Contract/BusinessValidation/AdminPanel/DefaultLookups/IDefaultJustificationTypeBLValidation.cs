using Dawem.Models.Dtos.AdminPanel.DefaultLookups.DefaultJustificationsTypes;

namespace Dawem.Contract.BusinessValidation.AdminPanel.DefaultLookups
{
    public interface IDefaultJustificationTypeBLValidation
    {
        Task<bool> CreateValidation(CreateDefaultJustificationsTypeDTO model);
        Task<bool> UpdateValidation(UpdateDefaultJustificationsTypeDTO model);
    }
}
