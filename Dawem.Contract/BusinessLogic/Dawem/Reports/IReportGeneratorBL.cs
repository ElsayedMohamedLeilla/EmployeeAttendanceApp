using Dawem.Enums.Generals;
using Dawem.Models.Dtos.Dawem.Reports.ExporterModel;
using Dawem.Models.Response.Dawem.ReportCritrias;
using Dawem.Models.Response.Dawem.ReportCritrias.AttendanceReports;


namespace Dawem.Contract.BusinessLogic.Dawem.Reports
{
    public interface IReportGeneratorBL
    {
        #region Attendance Report
        HttpResponseMessage GenerateEmployeeDailyAttendanceGroupByDay(EmployeeDailyAttendanceGroupByDayReportCritria param);
        HttpResponseMessage GenerateAttendaceLeaveStatusByDepartmentID(EmployeeAttendanceByDepartmentReportCritria param);
        HttpResponseMessage GenerateAttendaceLeaveStatusShortGroupByJobReport(AttendaceLeaveStatusShortGroupByJobReportCritria param);
        HttpResponseMessage GenerateAttendanceDetailsByEmployeeIDReport(AttendanceDetailsByEmployeeIDReportCritria param);
        HttpResponseMessage GenerateLateEarlyArrivalGroupByDepartmentReport(LateEarlyArrivalGroupByDepartmentReportCritria param);
        HttpResponseMessage GenerateLateEarlyArrivalGroupByEmployeeReport(LateEarlyArrivalGroupByEmployeeReportCritria param);
        HttpResponseMessage GenerateEmployeeAbsenseInPeriodGroupByEmployeeReport(EmployeeAbsenseInPeriodGroupByEmployeeReportCritria param);
        HttpResponseMessage GenerateEmployeeAbsenseInPeriodGroupByDepartmentReport(EmployeeAbsenseInPeriodGroupByDepartmentReportCritria param);
        HttpResponseMessage GenerateOverTimeInSelectedPeriodReport(OverTimeInSelectedPeriodReportCritria param);

        HttpResponseMessage GenerateAttendaceLeaveSummaryReport(AttendaceLeaveSummaryReportCritria param);
        #endregion


        #region Summon Reports
        HttpResponseMessage GenerateBriefingSummonsInPeriodReport(BriefingSummonsInPeriodReportCritria param);
        HttpResponseMessage GenerateSummonsDetailsInPeriodReport(SummonsDetailsInPeriodReportCritria param);
        HttpResponseMessage GenerateSummonsDetailsGroupByEmployeeReport(SummonsDetailsGroupByEmployeeReportCritria param);

        #endregion

        #region Statistics
        HttpResponseMessage GenerateStatisticsOverAperiodReport(StatisticsOverAperiodReportCritria param);
        HttpResponseMessage GenerateStatisticsReportOverAperiodByDepartmentReport(StatisticsReportOverAperiodByDepartmentReportCritria param);
        HttpResponseMessage GenerateStatisticsReportOverAperiodGroupByMonthReport(StatisticsReportOverAperiodGroupByMonthReportCritria param);

        #endregion


        //Report GenerateReport(ExporterModelDTO exporterModelDTO, BaseReportCritria param);
        public IEnumerable<dynamic> GetDataSource(object[] parameters, ReportType reportType);




    }
}
