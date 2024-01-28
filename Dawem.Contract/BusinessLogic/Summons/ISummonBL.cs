using Dawem.Models.Dtos.Summons.Summons;
using Dawem.Models.Response.Summons.Summons;

namespace Dawem.Contract.BusinessLogic.Summons
{
    public interface ISummonBL
    {
        Task<int> Create(CreateSummonModel model);
        Task<bool> Update(UpdateSummonModel model);
        Task<GetSummonInfoResponseModel> GetInfo(int fingerprintEnforcementId);
        Task<GetSummonByIdResponseModel> GetById(int fingerprintEnforcementId);
        Task<GetSummonsResponse> Get(GetSummonsCriteria model);
        Task<bool> Delete(int fingerprintEnforcementId);
        Task<GetSummonsInformationsResponseDTO> GetSummonsInformations();
    }
}
