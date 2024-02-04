using Dawem.Models.Dtos.Employees.Employees;
using Dawem.Models.Dtos.Summons.Summons;
using Dawem.Models.Response.Summons.Summons;

namespace Dawem.Contract.BusinessLogic.Summons
{
    public interface ISummonBL
    {
        Task<int> Create(CreateSummonModel model);
        Task<bool> Update(UpdateSummonModel model);
        Task<GetSummonInfoResponseModel> GetInfo(int summonId);
        Task<GetSummonByIdResponseModel> GetById(int summonId);
        Task<GetSummonsResponse> Get(GetSummonsCriteria model);
        Task<bool> Disable(DisableModelDTO model);
        Task<bool> Enable(int summonId);
        Task<bool> Delete(int summonId);
        Task<GetSummonsInformationsResponseDTO> GetSummonsInformations();
        Task HandleSummonMissingLog();
    }
}
