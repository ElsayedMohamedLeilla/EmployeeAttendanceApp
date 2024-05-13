using Dawem.Enums.Generals;
using Dawem.Models.Dtos.Dawem.Reports.ExporterModel;
using Dawem.Models.Response.Dawem.Attendances;
using DocumentFormat.OpenXml.InkML;

namespace Dawem.Contract.BusinessLogic.Dawem.Reports
{
    public interface IReportGeneratorBL
    {
        //1
        HttpResponseMessage GenerateEmployeeDailyAttendanceGroupByDay(GetEmployeeAttendanceInPeriodReportParameters param);
        HttpResponseMessage GenerateAttendaceLeaveStatusByDepartmentID(GetEmployeeAttendanceInPeriodReportParameters param);
        HttpResponseMessage GenerateAttendaceLeaveStatusShortGroupByJobReport(GetEmployeeAttendanceInPeriodReportParameters param);
        HttpResponseMessage GenerateAttendanceDetailsByEmployeeIDReport(GetEmployeeAttendanceInPeriodReportParameters param);
        HttpResponseMessage GenerateLateEarlyArrivalGroupByDepartmentReport(GetEmployeeAttendanceInPeriodReportParameters param);
        HttpResponseMessage GenerateReport(ExporterModelDTO exporterModelDTO, GetEmployeeAttendanceInPeriodReportParameters param);
        public IEnumerable<dynamic> GetDataSource(object[] parameters, ReportType reportType);
        
           


    }
}
