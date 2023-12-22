using Dawem.Models.Dtos.Employees.HolidayType;
using Dawem.Models.Response.Employees.HolidayTypes;
using Dawem.Models.Response.Requests.Vacations;

namespace Dawem.Contract.BusinessLogic.Employees
{
    public interface IHolidayTypeBL
    {
        Task<int> Create(CreateHolidayTypeModel model);
        Task<bool> Update(UpdateHolidayTypeModel model);
        Task<GetHolidayTypeInfoResponseModel> GetInfo(int holidayTypeId);
        Task<GetHolidayTypeByIdResponseModel> GetById(int holidayTypeId);
        Task<GetHolidayTypesResponse> Get(GetHolidayTypesCriteria model);
        Task<GetHolidayTypesForDropDownResponse> GetForDropDown(GetHolidayTypesCriteria model);
        Task<bool> Delete(int holidayTypeId);
        Task<GetHolidayTypesInformationsResponseDTO> GetHolidayTypesInformations();
    }
}
