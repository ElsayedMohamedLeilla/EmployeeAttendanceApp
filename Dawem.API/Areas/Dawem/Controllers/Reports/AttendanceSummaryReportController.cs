using Dawem.Contract.BusinessLogic.Dawem.Reports;
using Dawem.Models.Dtos.Dawem.Reports.AttendanceSummaryReport;
using Dawem.Translations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dawem.API.Areas.Dawem.Controllers.Reports
{
    [Route(LeillaKeys.DawemApiControllerAction), ApiController, Authorize, DawemAuthorize]
    
    
    public class AttendanceSummaryReportController : DawemControllerBase
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
