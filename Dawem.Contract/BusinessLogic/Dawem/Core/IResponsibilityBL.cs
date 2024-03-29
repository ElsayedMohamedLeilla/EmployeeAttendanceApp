using Dawem.Models.Dtos.Dawem.Core.Responsibilities;
using Dawem.Models.Response.Dawem.Employees.JobTitles;

namespace Dawem.Contract.BusinessLogic.Dawem.Core
{
    public interface IResponsibilityBL
    {
        Task<int> Create(CreateResponsibilityModel model);
        Task<bool> Update(UpdateResponsibilityModel model);
        Task<GetResponsibilityInfoResponseModel> GetInfo(int responsibilityId);
        Task<GetResponsibilityByIdResponseModel> GetById(int responsibilityId);
        Task<GetResponsibilitiesResponse> Get(GetResponsibilitiesCriteria model);
        Task<GetResponsibilitiesForDropDownResponse> GetForDropDown(GetResponsibilitiesCriteria model);
        Task<bool> Delete(int responsibilityId);
        Task<GetResponsibilitiesInformationsResponseDTO> GetResponsibilitiesInformations();
    }
}
