using Dawem.Models.Response.Dawem.Attendances;

namespace Dawem.Contract.BusinessLogic.Dawem.Reports
{
    public interface IReportGeneratorBL
    {
        HttpResponseMessage GenerateAttendanceForAllEmployeeReport(GetEmployeeAttendanceInPeriodReportParameters param);
        HttpResponseMessage GenerateAttendaceLeaveStatusByDepartmentID(GetEmployeeAttendanceInPeriodReportParameters param);
        HttpResponseMessage GenerateAttendaceLeaveStatusByEmployeeID(GetEmployeeAttendanceInPeriodReportParameters param);
        HttpResponseMessage GenerateAttendaceLeaveSummary(GetEmployeeAttendanceInPeriodReportParameters param);

    }
}
