using Dawem.Models.Dtos.Employees.Employees;
using Dawem.Models.Dtos.Schedules.ShiftWorkingTimes;
using Dawem.Models.Response.Schedules.ShiftWorkingTimes;

namespace Dawem.Contract.BusinessLogic.Schedules.ShiftWorkingTime
{
    public interface IShiftWorkingTimeBL
    {
        Task<int> Create(CreateShiftWorkingTimeModelDTO model);
        Task<bool> Update(UpdateShiftWorkingTimeModelDTO model);
        Task<GetShiftWorkingTimeInfoResponseDTO> GetInfo(int ShiftWorkingTimeId);
        Task<GetShiftWorkingTimeByIdResponseDTO> GetById(int ShiftWorkingTimeId);
        Task<GetShiftWorkingTimeResponseDTO> Get(GetShiftWorkingTimesCriteria model);
        Task<GetShiftWorkingTimeDropDownResponseDTO> GetForDropDown(GetShiftWorkingTimesCriteria model);
        Task<bool> Delete(int ShiftWorkingTimeId);
        Task<bool> Enable(int ShiftWorkingTimeId);
        Task<bool> Disable(DisableModelDTO model);
        Task<GetShiftWorkingTimesInformationsResponseDTO> GetShiftWorkingTimesInformations();
    }
}
