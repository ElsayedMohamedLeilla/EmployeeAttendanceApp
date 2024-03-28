using Dawem.Models.Dtos.Summons.Summons;
using Dawem.Models.Response.Summons.Summons;

namespace Dawem.Contract.BusinessLogic.Summons
{
    public interface ISummonLogBL
    {
        Task<GetSummonLogInfoResponseModel> GetInfo(int summonLogId);
        Task<GetSummonLogsResponse> Get(GetSummonLogsCriteria model);
        Task<GetSummonLogsInformationsResponseDTO> GetSummonLogsInformations();
    }
}
