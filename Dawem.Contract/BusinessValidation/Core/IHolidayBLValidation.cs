using Dawem.Models.Dtos.Core.Holidays;

namespace Dawem.Contract.BusinessValidation.Core
{
    public interface IHolidayBLValidation
    {
        Task<bool> CreateValidation(CreateHolidayDTO model);
        Task<bool> UpdateValidation(UpdateHolidayDTO model);
    }
}
