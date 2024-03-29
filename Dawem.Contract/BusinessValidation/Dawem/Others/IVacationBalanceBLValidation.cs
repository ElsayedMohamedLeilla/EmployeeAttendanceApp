using Dawem.Models.Dtos.Dawem.Others.VacationBalances;

namespace Dawem.Contract.BusinessValidation.Dawem.Others
{
    public interface IVacationBalanceBLValidation
    {
        Task<bool> CreateValidation(CreateVacationBalanceModel model);
        Task<bool> UpdateValidation(UpdateVacationBalanceModel model);
    }
}
