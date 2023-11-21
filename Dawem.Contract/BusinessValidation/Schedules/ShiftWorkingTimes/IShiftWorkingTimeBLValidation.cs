using Dawem.Models.Dtos.Schedules.ShiftWorkingTimes;

namespace Dawem.Contract.BusinessValidation.Schedules.ShiftWorkingTimes
{
    public interface IShiftWorkingTimeBLValidation
    {
        Task<bool> CreateValidation(CreateShiftWorkingTimeModelDTO model);
        Task<bool> UpdateValidation(UpdateShiftWorkingTimeModelDTO model);
    }
}
