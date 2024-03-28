using Dawem.Models.Dtos.Dawem.Core.Holidays;

namespace Dawem.Contract.BusinessValidation.Dawem.Core
{
    public interface IHolidayBLValidation
    {
        Task<bool> CreateValidation(CreateHolidayDTO model);
        Task<bool> UpdateValidation(UpdateHolidayDTO model);
    }
}
