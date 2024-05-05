using Dawem.Contract.BusinessLogic.Dawem.Attendances;
using Dawem.Contract.BusinessLogic.Dawem.Reports;
using Dawem.Models.Dtos.Dawem.Reports.AttendanceSummaryReport;
using Dawem.Models.Response.Dawem.Attendances;
using Dawem.Translations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dawem.API.Areas.Dawem.Controllers.Reports
{
    [Route(LeillaKeys.DawemApiControllerAction), ApiController, Authorize, DawemAuthorize]
    
    
    public class ReportController : DawemControllerBase
    {
        private readonly IEmployeeAttendanceBL employeeAttendanceBL;
        public ReportController(IEmployeeAttendanceBL _employeeAttendanceBL)
        {
            employeeAttendanceBL = _employeeAttendanceBL;
        }
        //[HttpGet]
        //public async Task<ActionResult> GetAttendanceReport(GetEmployeeAttendanceInPeriodReportParameters model)
        //{
        //    var result = await employeeAttendanceBL.GetEmployeeAttendanceInPeriodReport(model);

        //    //return Success(result.AttendanceSmmaries, result.TotalCount);
        //}
    }
}
