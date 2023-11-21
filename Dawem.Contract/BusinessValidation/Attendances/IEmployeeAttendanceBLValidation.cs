using Dawem.Models.Dtos.Employees.HolidayType;
using Dawem.Models.Response.Schedules.Schedules;

namespace Dawem.Contract.BusinessValidation.Schedules.Schedules
{
    public interface IEmployeeAttendanceBLValidation
    {
        Task<FingerPrintValidationResponseModel> FingerPrintValidation();
        Task<GetCurrentAttendanceInfoResponseModel> GetCurrentAttendanceInfoValidation();
        Task<bool> GetCurrentEmployeeAttendancesValidation(GetEmployeeAttendancesCriteria model);
    }
}
