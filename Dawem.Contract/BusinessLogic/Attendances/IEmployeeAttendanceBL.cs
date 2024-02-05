using Dawem.Enums.Generals;
using Dawem.Models.Dtos.Attendances;
using Dawem.Models.Response.Attendances;
using Dawem.Models.Response.Dashboard;

namespace Dawem.Contract.BusinessLogic.Attendances
{
    public interface IEmployeeAttendanceBL
    {
        Task<FingerPrintType> CreateFingerPrint(FingerprintModel model);
        Task<GetCurrentFingerPrintInfoResponseModel> GetCurrentFingerPrintInfo();
        Task<List<GetEmployeeAttendancesResponseModel>> GetEmployeeAttendances(GetEmployeeAttendancesCriteria model);
        Task<GetEmployeeAttendancesResponseForWebDTO> GetEmployeeAttendancesForWebAdmin(GetEmployeeAttendancesForWebAdminCriteria model);
        Task<List<GetEmployeeAttendanceInfoDTO>> GetEmployeeAttendancesInfo(int employeeAttendanceId);
        Task<bool> Delete(DeleteEmployeeAttendanceModel model);
        Task<GetEmployeesAttendancesInformationsResponseModel> GetEmployeesAttendancesInformations();

    }
}
