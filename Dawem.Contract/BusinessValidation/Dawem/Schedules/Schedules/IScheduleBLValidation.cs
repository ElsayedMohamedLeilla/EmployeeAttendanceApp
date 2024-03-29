using Dawem.Models.Dtos.Dawem.Schedules.Schedules;

namespace Dawem.Contract.BusinessValidation.Dawem.Schedules.Schedules
{
    public interface IScheduleBLValidation
    {
        Task<bool> CreateValidation(CreateScheduleModel model);
        Task<bool> UpdateValidation(UpdateScheduleModel model);
    }
}
