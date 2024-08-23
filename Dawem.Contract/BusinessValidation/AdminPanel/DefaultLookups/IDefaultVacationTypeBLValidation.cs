using Dawem.Models.Dtos.AdminPanel.DefaultLookups.DefaultVacationsTypes;

namespace Dawem.Contract.BusinessValidation.Dawem.Core
{
    public interface IDefaultVacationTypeBLValidation
    {
        Task<bool> CreateValidation(CreateDefaultVacationsTypeDTO model);
        Task<bool> UpdateValidation(UpdateDefaultVacationsTypeDTO model);
    }
}
