using Dawem.Models.Dtos.Dawem.Summons.Summons;
using Dawem.Models.Response.Summons.SummonLogs;

namespace Dawem.Contract.BusinessLogic.Dawem.Summons
{
    public interface ISummonLogBL
    {
        Task<GetSummonLogInfoResponseModel> GetInfo(int summonLogId);
        Task<GetSummonLogsResponse> Get(GetSummonLogsCriteria model);
        Task<GetSummonLogsInformationsResponseDTO> GetSummonLogsInformations();
    }
}
