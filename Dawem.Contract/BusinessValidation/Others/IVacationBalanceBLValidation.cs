using Dawem.Models.Dtos.Schedules.SchedulePlans;

namespace Dawem.Contract.BusinessValidation.Schedules.SchedulePlans
{
    public interface IVacationBalanceBLValidation
    {
        Task<bool> CreateValidation(CreateVacationBalanceModel model);
        Task<bool> UpdateValidation(UpdateVacationBalanceModel model);
    }
}
