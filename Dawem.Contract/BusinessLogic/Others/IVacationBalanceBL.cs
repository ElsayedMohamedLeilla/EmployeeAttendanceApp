using Dawem.Models.Dtos.Others.VacationBalances;
using Dawem.Models.Dtos.Schedules.SchedulePlans;
using Dawem.Models.Response.Requests.Vacations;
using Dawem.Models.Response.Schedules.SchedulePlans;

namespace Dawem.Contract.BusinessLogic.Schedules.VacationBalances
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
