using Dawem.Models.Criteria.DefaultLookups;
using Dawem.Models.Dtos.AdminPanel.DefaultLookups.DefaultOfficialHolidaysTypes;
using Dawem.Models.Dtos.Dawem.Employees.Employees;
using Dawem.Models.Response.AdminPanel.DefaultLookups.DefaultOfficialHolidaysTypes;

namespace Dawem.Contract.BusinessLogic.AdminPanel.DefaultLookups
{
    public interface IDefaultOfficialHolidayBL
    {
        Task<int> Create(CreateDefaultOfficialHolidaysDTO model);
        Task<bool> Update(UpdateDefaultOfficialHolidaysDTO model);
        Task<GetDefaultOfficialHolidaysInfoResponseDTO> GetInfo(int DefaultOfficialHolidaysTypeId);
        Task<GetDefaultOfficialHolidaysByIdResponseDTO> GetById(int DefaultOfficialHolidaysTypeId);
        Task<GetDefaultOfficialHolidaysResponseDTO> Get(GetDefaultOfficialHolidayCriteria model);
        Task<GetDefaultOfficialHolidaysDropDownResponseDTO> GetForDropDown(GetDefaultOfficialHolidayCriteria model);
        Task<bool> Delete(int DefaultOfficialHolidaysTypeId);

        public Task<bool> Enable(int GroupId);

        public Task<bool> Disable(DisableModelDTO model);


    }
}
