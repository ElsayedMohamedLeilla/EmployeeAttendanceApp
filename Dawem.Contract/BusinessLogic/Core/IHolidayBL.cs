using Dawem.Models.Criteria.Core;
using Dawem.Models.Dtos.Core.Holidays;
using Dawem.Models.Dtos.Employees.Employees;
using Dawem.Models.Response.Core.Holidays;

namespace Dawem.Contract.BusinessLogic.Core
{
    public interface IHolidayBL
    {
        Task<int> Create(CreateHolidayDTO model);
        Task<bool> Update(UpdateHolidayDTO model);
        Task<GetHolidayInfoResponseDTO> GetInfo(int HolidayId);
        Task<GetHolidayByIdResponseDTO> GetById(int HolidayId);
        Task<GetHolidayResponseDTO> Get(GetHolidayCriteria model);
        Task<GetHolidayDropDownResponseDTO> GetForDropDown(GetHolidayCriteria model);
        Task<bool> Delete(int HolidayId);
        Task<bool> Enable(int HolidayId);
        Task<bool> Disable(DisableModelDTO model);

        Task<GetHolidaiesInformationsResponseDTO> GetHolidaiesInformation();

        Task<GetHolidayResponseForEmployeeDTO> GetForEmployee();


    }
}
