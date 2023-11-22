using Dawem.Enums.Generals;
using Dawem.Models.Dtos.Employees.HolidayType;
using Dawem.Models.Response.Schedules.Schedules;

namespace Dawem.Contract.BusinessLogic.Schedules.Schedules
{
    public interface IEmployeeAttendanceBL
    {
        Task<FingerPrintType> FingerPrint(FingerprintModel model);
        Task<GetCurrentFingerPrintInfoResponseModel> GetCurrentFingerPrintInfo();
        Task<List<GetEmployeeAttendancesResponseModel>> GetEmployeeAttendances(GetEmployeeAttendancesCriteria model);
    }
}
