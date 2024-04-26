using Dawem.Models.Dtos.Dawem.Attendances;
using Dawem.Models.Response.Dawem.Attendances;

namespace Dawem.Contract.BusinessValidation.Dawem.Attendances
{
    public interface IEmployeeAttendanceBLValidation
    {
        Task<FingerPrintValidationResponseModel> FingerPrintValidation(FingerprintModel model);
        Task<GetCurrentFingerPrintInfoResponseModel> GetCurrentFingerPrintInfoValidation();
        Task<bool> GetEmployeeAttendancesValidation(GetEmployeeAttendancesCriteria model);
        Task<int> GetEmployeeScheduleValidation();
    }
}
