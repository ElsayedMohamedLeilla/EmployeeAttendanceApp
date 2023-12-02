using Dawem.Models.Dtos.Attendances;
using Dawem.Models.Response.Attendances;

namespace Dawem.Contract.BusinessLogic.Attendances
{
    public interface IEmployeeAttendanceBL
    {
        Task<bool> FingerPrint(FingerprintModel model);
        Task<GetCurrentFingerPrintInfoResponseModel> GetCurrentFingerPrintInfo();
        Task<List<GetEmployeeAttendancesResponseModel>> GetEmployeeAttendances(GetEmployeeAttendancesCriteria model);
        Task<List<GetEmployeeAttendancesResponseForWebAdminModelDTO>> GetEmployeeAttendancesForWebAdmin(GetEmployeeAttendancesForWebAdminCriteria model);
    }
}
