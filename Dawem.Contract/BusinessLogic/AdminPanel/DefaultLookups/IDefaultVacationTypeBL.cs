using Dawem.Models.Criteria.DefaultLookups;
using Dawem.Models.Dtos.AdminPanel.DefaultLookups.DefaultVacationsTypes;
using Dawem.Models.Response.AdminPanel.DefaultLookups.DefaultVacationsTypes;

namespace Dawem.Contract.BusinessLogic.AdminPanel.DefaultLookups
{
    public interface IDefaultVacationTypeBL
    {
        Task<int> Create(CreateDefaultVacationsTypeDTO model);
        Task<bool> Update(UpdateDefaultVacationsTypeDTO model);
        Task<GetDefaultVacationsTypeInfoResponseDTO> GetInfo(int DefaultVacationsTypeId);
        Task<GetDefaultVacationsTypeByIdResponseDTO> GetById(int DefaultVacationsTypeId);
        Task<GetDefaultVacationsTypeResponseDTO> Get(GetDefaultVacationTypeCriteria model);
        Task<GetDefaultVacationsTypeDropDownResponseDTO> GetForDropDown(GetDefaultVacationTypeCriteria model);
        Task<bool> Delete(int DefaultVacationsTypeId);

    }
}
