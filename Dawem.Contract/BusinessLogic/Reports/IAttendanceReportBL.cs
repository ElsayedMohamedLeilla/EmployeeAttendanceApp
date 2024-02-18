using Dawem.Models.Dtos.Reports.AttendanceSummaryReport;

namespace Dawem.Contract.BusinessLogic.Employees
{
    public interface IAttendanceReportBL
    {
        Task<AttendanceSummaryResponseDTO> GetAttendanceSummary(AttendanceSummaryCritria model);
    }
}
