using Dawem.Models.Dtos.Employees.HolidayType;
using Dawem.Models.Response.Schedules.Schedules;

namespace Dawem.Contract.BusinessLogic.Schedules.Schedules
{
    public interface IEmployeeAttendanceBL
    {
        Task<bool> FingerPrint();
        Task<GetCurrentAttendanceInfoResponseModel> GetCurrentAttendanceInfo();
        Task<List<GetEmployeeAttendancesResponseModel>> GetCurrentEmployeeAttendances(GetEmployeeAttendancesCriteria model);
    }
}
