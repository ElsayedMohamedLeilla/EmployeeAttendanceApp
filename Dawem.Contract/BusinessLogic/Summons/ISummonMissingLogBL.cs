using Dawem.Models.Dtos.Summons.Summons;
using Dawem.Models.Response.Summons.Summons;

namespace Dawem.Contract.BusinessLogic.Summons
{
    public interface ISummonMissingLogBL
    {
        Task<GetSummonMissingLogInfoResponseModel> GetInfo(int summonMissingLogId);
        Task<GetSummonMissingLogsResponse> Get(GetSummonMissingLogsCriteria model);
        Task<GetSummonMissingLogsInformationsResponseDTO> GetSummonMissingLogsInformations();
    }
}
