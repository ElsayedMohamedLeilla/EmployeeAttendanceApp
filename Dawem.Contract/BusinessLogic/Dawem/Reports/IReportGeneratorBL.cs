using Dawem.Enums.Generals;
using Dawem.Models.Dtos.Dawem.Reports.ExporterModel;
using Dawem.Models.Response.Dawem.Attendances;
using DocumentFormat.OpenXml.InkML;

namespace Dawem.Contract.BusinessLogic.Dawem.Reports
{
    public interface IReportGeneratorBL
    {
        //1
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

        HttpResponseMessage GenerateReport(ExporterModelDTO exporterModelDTO, ReportCritria param);
        public IEnumerable<dynamic> GetDataSource(object[] parameters, ReportType reportType);
        
           


    }
}
