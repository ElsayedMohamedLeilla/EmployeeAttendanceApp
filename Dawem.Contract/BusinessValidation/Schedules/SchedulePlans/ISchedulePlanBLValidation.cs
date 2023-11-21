using Dawem.Models.Dtos.Schedules.SchedulePlans;

namespace Dawem.Contract.BusinessValidation.Schedules.SchedulePlans
{
    public interface ISchedulePlanBLValidation
    {
        Task<bool> CreateValidation(CreateSchedulePlanModel model);
        Task<bool> UpdateValidation(UpdateSchedulePlanModel model);
    }
}
