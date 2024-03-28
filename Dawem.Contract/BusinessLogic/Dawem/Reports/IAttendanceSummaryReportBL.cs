using Dawem.Models.Dtos.Dawem.Reports.AttendanceSummaryReport;

namespace Dawem.Contract.BusinessLogic.Dawem.Reports
{
    public interface IAttendanceSummaryReportBL
    {
        Task<AttendanceSummaryResponseDTO> GetAttendanceSummary(AttendanceSummaryCritria model);
    }
}
