using Dawem.Models.Dtos.Attendances.Schedules;

namespace Dawem.Contract.BusinessValidation.Attendances.Schedules
{
    public interface IScheduleBLValidation
    {
        Task<bool> CreateValidation(CreateScheduleModel model);
        Task<bool> UpdateValidation(UpdateScheduleModel model);
    }
}
