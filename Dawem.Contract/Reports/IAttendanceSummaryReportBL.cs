using Dawem.Models.Dtos.Reports.AttendanceSummaryReport;

namespace Dawem.Contract.Reports
{
    public interface IAttendanceSummaryReportBL
    {
        Task<AttendanceSummaryForGridDTO> Get(AttendanceSummaryCritria model);
    }
}
