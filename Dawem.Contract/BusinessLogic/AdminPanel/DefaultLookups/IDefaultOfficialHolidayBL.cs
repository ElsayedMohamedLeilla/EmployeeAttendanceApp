using Dawem.Models.Criteria.DefaultLookups;
using Dawem.Models.Dtos.AdminPanel.DefaultLookups.DefaultOfficialHolidaysTypes;
using Dawem.Models.Dtos.Dawem.Employees.Employees;
using Dawem.Models.Response.AdminPanel.DefaultLookups.DefaultOfficialHolidaysTypes;

namespace Dawem.Contract.BusinessLogic.AdminPanel.DefaultLookups
{
    public interface IDefaultOfficialHolidayTypeBL
    {
        Task<int> Create(CreateDefaultOfficialHolidaysDTO model);
        Task<bool> Update(UpdateDefaultOfficialHolidaysDTO model);
        Task<GetDefaultOfficialHolidaysTypeInfoResponseDTO> GetInfo(int DefaultOfficialHolidaysTypeId);
        Task<GetDefaultOfficialHolidaysTypeByIdResponseDTO> GetById(int DefaultOfficialHolidaysTypeId);
        Task<GetDefaultOfficialHolidaysTypeResponseDTO> Get(GetDefaultOfficialHolidayTypeCriteria model);
        Task<GetDefaultOfficialHolidaysTypeDropDownResponseDTO> GetForDropDown(GetDefaultOfficialHolidayTypeCriteria model);
        Task<bool> Delete(int DefaultOfficialHolidaysTypeId);

        public Task<bool> Enable(int GroupId);

        public Task<bool> Disable(DisableModelDTO model);


    }
}
