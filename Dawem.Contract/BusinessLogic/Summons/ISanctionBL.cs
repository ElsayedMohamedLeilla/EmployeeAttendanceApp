using Dawem.Models.Dtos.Employees.Employees;
using Dawem.Models.Dtos.Summons.Sanctions;
using Dawem.Models.Response.Summons.Sanctions;

namespace Dawem.Contract.BusinessLogic.Summons
{
    public interface ISanctionBL
    {
        Task<int> Create(CreateSanctionModel model);
        Task<bool> Update(UpdateSanctionModel model);
        Task<GetSanctionInfoResponseModel> GetInfo(int sanctionId);
        Task<GetSanctionByIdResponseModel> GetById(int sanctionId);
        Task<GetSanctionsResponse> Get(GetSanctionsCriteria model);
        Task<GetSanctionsForDropDownResponse> GetForDropDown(GetSanctionsCriteria model);
        Task<bool> Delete(int sanctionId);
        Task<bool> Disable(DisableModelDTO model);
        Task<bool> Enable(int sanctionId);
        Task<GetSanctionsInformationsResponseDTO> GetSanctionsInformations();
    }
}
