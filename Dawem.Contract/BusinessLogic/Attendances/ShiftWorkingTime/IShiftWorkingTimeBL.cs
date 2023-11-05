using Dawem.Models.Dtos.Employees.Attendance.ShiftWorkingTimes;
using Dawem.Models.Dtos.Employees.Employees;
using Dawem.Models.Response.Core.ShiftWorkingTimes;

namespace Dawem.Contract.BusinessLogic.Core
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
    }
}
