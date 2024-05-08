using Dawem.Models.Dtos.Dawem.Reports.ExporterModel;
using Dawem.Models.Response.Dawem.Attendances;

namespace Dawem.Contract.BusinessLogic.Dawem.Reports
{
    public interface IReportGeneratorBL
    {
        //1
        HttpResponseMessage GenerateEmployeeDailyAttendanceGroupByDay(GetEmployeeAttendanceInPeriodReportParameters param);

        HttpResponseMessage GenerateAttendaceLeaveStatusByDepartmentID(GetEmployeeAttendanceInPeriodReportParameters param);
        HttpResponseMessage GenerateAttendaceLeaveStatusByEmployeeID(GetEmployeeAttendanceInPeriodReportParameters param);
        HttpResponseMessage GenerateAttendaceLeaveSummary(GetEmployeeAttendanceInPeriodReportParameters param);
        HttpResponseMessage GenerateReport(ExporterModelDTO exporterModelDTO, GetEmployeeAttendanceInPeriodReportParameters param);

        
    }
}
