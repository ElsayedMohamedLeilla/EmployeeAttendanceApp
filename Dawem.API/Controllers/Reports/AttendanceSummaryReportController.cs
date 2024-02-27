using Dawem.Contract.BusinessLogic.Employees;
using Dawem.Models.Dtos.Reports.AttendanceSummaryReport;
using Dawem.Translations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dawem.API.Controllers.Core
{
    [Route(LeillaKeys.ApiControllerAction)]
    [ApiController]
    [Authorize]
    public class AttendanceSummaryReportController : BaseController
    {
        private readonly IAttendanceSummaryReportBL attendanceSummaryReportBL;
        public AttendanceSummaryReportController(IAttendanceSummaryReportBL _attendanceSummaryReportBL)
        {
            attendanceSummaryReportBL = _attendanceSummaryReportBL;
        }
        [HttpGet]
        public async Task<ActionResult> GetAttendanceSummary([FromQuery] AttendanceSummaryCritria model)
        {
            var result = await attendanceSummaryReportBL.GetAttendanceSummary(model);
            return Success(result.AttendanceSmmaries, result.TotalCount);
        }
    }
}
