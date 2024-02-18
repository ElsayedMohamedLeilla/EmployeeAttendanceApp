using Dawem.Contract.BusinessLogic.Core;
using Dawem.Contract.BusinessLogic.Employees;
using Dawem.Contract.Reports;
using Dawem.Models.Dtos.Reports.AttendanceSummaryReport;
using Dawem.Translations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dawem.API.Controllers.Core
{

    [Route(LeillaKeys.ApiControllerAction)]
    [ApiController]
    [Authorize]
    public class AttendanceReportController : BaseController
    {
        private readonly IAttendanceSummaryReportBL attendanceSummaryReportBL;
        private readonly IAttendanceReportBL attendanceReportBL;
        public AttendanceReportController(IAttendanceSummaryReportBL _attendanceSummaryReportBL, IAttendanceReportBL _attendanceReportBL)
        {
            attendanceSummaryReportBL = _attendanceSummaryReportBL;
            attendanceReportBL = _attendanceReportBL;
        }
        [HttpGet]
        public async Task<ActionResult> GetAttendanceSummary([FromQuery] AttendanceSummaryCritria model)
        {
            var result = await attendanceSummaryReportBL.Get(model);
            return Success(result);
        }
        [HttpGet]
        public async Task<ActionResult> GetAttendanceSummaryNew([FromQuery] AttendanceSummaryCritria model)
        {
            var result = await attendanceReportBL.GetAttendanceSummary(model);
            return Success(result);
        }
    }
}
