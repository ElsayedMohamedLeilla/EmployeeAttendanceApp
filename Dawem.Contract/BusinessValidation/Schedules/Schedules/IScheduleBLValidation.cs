using Dawem.Models.Dtos.Schedules.Schedules;

namespace Dawem.Contract.BusinessValidation.Schedules.Schedules
{
    public interface IScheduleBLValidation
    {
        Task<bool> CreateValidation(CreateScheduleModel model);
        Task<bool> UpdateValidation(UpdateScheduleModel model);
    }
}
