using Dawem.Models.Dtos.Employees.Employees;

namespace Dawem.Contract.BusinessValidation.Attendances.SchedulePlans
{
    public interface ISchedulePlanBLValidation
    {
        Task<bool> CreateValidation(CreateSchedulePlanModel model);
        Task<bool> UpdateValidation(UpdateSchedulePlanModel model);
    }
}
