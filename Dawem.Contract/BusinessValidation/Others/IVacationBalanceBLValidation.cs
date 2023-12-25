using Dawem.Models.Dtos.Others.VacationBalances;

namespace Dawem.Contract.BusinessValidation.Others
{
    public interface IVacationBalanceBLValidation
    {
        Task<bool> CreateValidation(CreateVacationBalanceModel model);
        Task<bool> UpdateValidation(UpdateVacationBalanceModel model);
    }
}
