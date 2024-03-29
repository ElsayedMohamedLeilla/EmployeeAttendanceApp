using Dawem.Models.Dtos.Dawem.Schedules.ShiftWorkingTimes;

namespace Dawem.Contract.BusinessValidation.Dawem.Schedules.ShiftWorkingTimes
{
    public interface IShiftWorkingTimeBLValidation
    {
        Task<bool> CreateValidation(CreateShiftWorkingTimeModelDTO model);
        Task<bool> UpdateValidation(UpdateShiftWorkingTimeModelDTO model);
    }
}
