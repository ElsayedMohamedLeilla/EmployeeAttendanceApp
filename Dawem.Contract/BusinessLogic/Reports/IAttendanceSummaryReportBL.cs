using Dawem.Models.Dtos.Reports.AttendanceSummaryReport;

namespace Dawem.Contract.BusinessLogic.Reports
{
    public interface IAttendanceSummaryReportBL
    {
        Task<AttendanceSummaryResponseDTO> GetAttendanceSummary(AttendanceSummaryCritria model);
    }
}
