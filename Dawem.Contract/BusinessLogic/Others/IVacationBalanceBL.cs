using Dawem.Models.Dtos.Others.VacationBalances;
using Dawem.Models.Response.Others.VacationBalances;

namespace Dawem.Contract.BusinessLogic.Others
{
    public interface IVacationBalanceBL
    {
        Task<bool> Create(CreateVacationBalanceModel model);
        Task<bool> Update(UpdateVacationBalanceModel model);
        Task<GetVacationBalanceInfoResponseModel> GetInfo(int vacationBalanceId);
        Task<GetVacationBalanceByIdResponseModel> GetById(int vacationBalanceId);
        Task<GetVacationBalancesResponse> Get(GetVacationBalancesCriteria model);
        Task<bool> Delete(int vacationBalanceId);
        Task<GetVacationBalancesInformationsResponseDTO> GetVacationBalancesInformations();
    }
}
