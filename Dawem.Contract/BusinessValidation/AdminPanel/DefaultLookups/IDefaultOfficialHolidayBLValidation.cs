using Dawem.Models.Dtos.AdminPanel.DefaultLookups.DefaultOfficialHolidaysTypes;

namespace Dawem.Contract.BusinessValidation.AdminPanel.DefaultLookups
{
    public interface IDefaultOfficialHolidayBLValidation
    {
        Task<bool> CreateValidation(CreateDefaultOfficialHolidaysDTO model);
        Task<bool> UpdateValidation(UpdateDefaultOfficialHolidaysDTO model);
    }
}
