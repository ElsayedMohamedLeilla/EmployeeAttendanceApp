using Dawem.Models.Dtos.Reports.AttendanceSummaryReport;

namespace Dawem.Contract.BusinessLogic.Employees
{
    public interface IAttendanceSummaryReportBL
    {
        Task<AttendanceSummaryResponseDTO> GetAttendanceSummary(AttendanceSummaryCritria model);
    }
}
