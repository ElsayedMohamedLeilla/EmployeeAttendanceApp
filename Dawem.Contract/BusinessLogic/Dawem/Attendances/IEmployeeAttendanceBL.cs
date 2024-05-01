using Dawem.Enums.Generals;
using Dawem.Models.Dtos.Dawem.Attendances;
using Dawem.Models.Response.Dawem.Attendances;
using Dawem.Models.Response.Dawem.Dashboard;

namespace Dawem.Contract.BusinessLogic.Dawem.Attendances
{
    public interface IEmployeeAttendanceBL
    {
        Task<FingerPrintType> CreateFingerPrint(FingerprintModel model);
        Task<GetCurrentFingerPrintInfoResponseModel> GetCurrentFingerPrintInfo();
        Task<List<GetEmployeeAttendancesResponseModel>> GetEmployeeAttendances(GetEmployeeAttendancesCriteria model);
        Task<GetEmployeeAttendancesResponseForWebDTO> GetEmployeeAttendancesForWebAdmin(GetEmployeeAttendancesForWebAdminCriteria model);
        Task<GetEmployeeAttendanceInfoDTO> GetEmployeeAttendancesInfo(int employeeAttendanceId);
        Task<bool> Delete(DeleteEmployeeAttendanceModel model);
        Task<GetEmployeesAttendancesInformationsResponseModel> GetEmployeesAttendancesInformations();
        public Task<MemoryStream> ExportDraft();
        public Task<Dictionary<string, string>> ImportDataFromExcelToDB(Stream importedFile);
        public Task<List<GetEmployeeAttendanceInPeriodReportModel>> GetEmployeeAttendanceInPeriodReport(GetEmployeeAttendanceInPeriodReportParameters Critria);



        Task<GetCurrentEmployeeSchedulesResponse> GetCurrentEmployeeSchedules(GetCurrentEmployeeSchedulesModel model);
    }
}
