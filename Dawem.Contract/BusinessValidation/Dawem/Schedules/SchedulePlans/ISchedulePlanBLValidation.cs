using Dawem.Models.Dtos.Dawem.Schedules.SchedulePlans;

namespace Dawem.Contract.BusinessValidation.Dawem.Schedules.SchedulePlans
{
    public interface ISchedulePlanBLValidation
    {
        Task<bool> CreateValidation(CreateSchedulePlanModel model);
        Task<bool> UpdateValidation(UpdateSchedulePlanModel model);
    }
}
