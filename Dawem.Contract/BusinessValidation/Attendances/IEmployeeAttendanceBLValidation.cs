using Dawem.Models.Dtos.Employees.HolidayType;
using Dawem.Models.Response.Schedules.Schedules;

namespace Dawem.Contract.BusinessValidation.Schedules.Schedules
{
    public interface IEmployeeAttendanceBLValidation
    {
        Task<FingerPrintValidationResponseModel> FingerPrintValidation(FingerprintModel model);
        Task<GetCurrentFingerPrintInfoResponseModel> GetCurrentFingerPrintInfoValidation();
        Task<bool> GetEmployeeAttendancesValidation(GetEmployeeAttendancesCriteria model);
    }
}
