using Dawem.Models.Dtos.Dawem.Employees.HolidayTypes;
using Dawem.Models.Response.Dawem.Employees.HolidayTypes;

namespace Dawem.Contract.BusinessLogic.Dawem.Employees
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
