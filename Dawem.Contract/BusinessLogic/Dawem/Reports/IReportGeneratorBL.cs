using Dawem.Enums.Generals;
using Dawem.Models.Dtos.Dawem.Reports.AttendanceSummaryReport;
using Dawem.Models.Dtos.Dawem.Reports.ExporterModel;
using Dawem.Models.Response.Dawem.Attendances;
using DocumentFormat.OpenXml.InkML;

namespace Dawem.Contract.BusinessLogic.Dawem.Reports
{
    public interface IReportGeneratorBL
    {
        #region Attendance Report
        HttpResponseMessage GenerateEmployeeDailyAttendanceGroupByDay(ReportCritria param);
        HttpResponseMessage GenerateAttendaceLeaveStatusByDepartmentID(ReportCritria param);
        HttpResponseMessage GenerateAttendaceLeaveStatusShortGroupByJobReport(ReportCritria param);
        HttpResponseMessage GenerateAttendanceDetailsByEmployeeIDReport(ReportCritria param);
        HttpResponseMessage GenerateLateEarlyArrivalGroupByDepartmentReport(ReportCritria param);
        HttpResponseMessage GenerateLateEarlyArrivalGroupByEmployeeReport(ReportCritria param);
        HttpResponseMessage GenerateEmployeeAbsenseInPeriodGroupByEmployeeReport(ReportCritria param);
        HttpResponseMessage GenerateEmployeeAbsenseInPeriodGroupByDepartmentReport(ReportCritria param);
        HttpResponseMessage GenerateOverTimeInSelectedPeriodReport(ReportCritria param);

        HttpResponseMessage GenerateAttendaceLeaveSummaryReport(ReportCritria param);
        #endregion


        #region Summon Reports
        HttpResponseMessage GenerateBriefingSummonsInPeriodReport(ReportCritria param);
        HttpResponseMessage GenerateSummonsDetailsInPeriodReport(ReportCritria param);
        HttpResponseMessage GenerateSummonsDetailsGroupByEmployeeReport(ReportCritria param);

        #endregion

        #region Statistics
        HttpResponseMessage GenerateStatisticsOverAperiodReport(ReportCritria param);
        HttpResponseMessage GenerateStatisticsReportOverAperiodByDepartmentReport(ReportCritria param);
        HttpResponseMessage GenerateStatisticsReportOverAperiodGroupByMonthReport(ReportCritria param);

        #endregion


        HttpResponseMessage GenerateReport(ExporterModelDTO exporterModelDTO, ReportCritria param);
        public IEnumerable<dynamic> GetDataSource(object[] parameters, ReportType reportType);
        
           


    }
}
