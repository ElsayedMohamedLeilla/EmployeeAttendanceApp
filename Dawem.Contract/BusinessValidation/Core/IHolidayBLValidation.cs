using Dawem.Models.Dtos.Core.Group;
using Dawem.Models.Dtos.Core.Holidaies;

namespace Dawem.Contract.BusinessValidation.Core
{
    public interface IHolidayBLValidation
    {
        Task<bool> CreateValidation(CreateHolidayDTO model);
        Task<bool> UpdateValidation(UpdateHolidayDTO model);
    }
}
