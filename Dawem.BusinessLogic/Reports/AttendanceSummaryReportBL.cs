using Dawem.Contract.Reports;
using Dawem.Models.Dtos.Reports.AttendanceSummaryReport;
using Dawem.Reports.Employees.AttendanceSummaryReport;

namespace Dawem.BusinessLogic.Reports
{
    public class AttendanceSummaryReportBL : IAttendanceSummaryReportBL
    {
        private readonly AttendanceSummaryHelper attendanceSummaryHelper;

        public AttendanceSummaryReportBL(AttendanceSummaryHelper _attendanceSummaryHelper)
        {
            attendanceSummaryHelper = _attendanceSummaryHelper;
        }
        public async Task<AttendanceSummaryResponseDTO> Get(AttendanceSummaryCritria model)
        {
            return await attendanceSummaryHelper.Get(model);
        }
    }
}
