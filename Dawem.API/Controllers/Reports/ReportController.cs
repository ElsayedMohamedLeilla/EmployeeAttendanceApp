using Dawem.Contract.BusinessLogic.Core;
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
    public class ReportController : BaseController
    {
        private readonly IAttendanceSummaryReportBL attendanceSummaryReportBL;
        public ReportController(IAttendanceSummaryReportBL _attendanceSummaryReportBL)
        {
            attendanceSummaryReportBL = _attendanceSummaryReportBL;
        }

        [HttpGet]
        public async Task<ActionResult> GetAttendanceSummary([FromQuery] AttendanceSummaryCritria model)
        {
            var result = await attendanceSummaryReportBL.Get(model);
            return Success(result);
        }
    }
}
