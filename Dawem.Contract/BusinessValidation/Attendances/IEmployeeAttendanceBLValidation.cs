using Dawem.Models.Dtos.Attendances;
using Dawem.Models.Response.Attendances;

namespace Dawem.Contract.BusinessValidation.Attendances
{
    public interface IEmployeeAttendanceBLValidation
    {
        Task<FingerPrintValidationResponseModel> FingerPrintValidation(FingerprintModel model);
        Task<GetCurrentFingerPrintInfoResponseModel> GetCurrentFingerPrintInfoValidation();
        Task<bool> GetEmployeeAttendancesValidation(GetEmployeeAttendancesCriteria model);
    }
}
