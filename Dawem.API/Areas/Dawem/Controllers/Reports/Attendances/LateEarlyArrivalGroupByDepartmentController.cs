using Dawem.Contract.BusinessLogic.Dawem.Reports;
using Dawem.Enums.Generals;
using Dawem.Models.Response.Dawem.ReportCritrias.AttendanceReports;
using Dawem.Translations;
using Microsoft.AspNetCore.Mvc;

namespace Dawem.API.Areas.Dawem.Controllers.Reports.Attendance
{


    [Route(LeillaKeys.DawemApiControllerAction), ApiController, DawemAuthorize]

    public class LateEarlyArrivalGroupByDepartmentController : DawemControllerBase
    {
        private readonly IReportGeneratorBL _reportGeneratorBL;
        public LateEarlyArrivalGroupByDepartmentController(IReportGeneratorBL reportGeneratorBL)
        {
            _reportGeneratorBL = reportGeneratorBL;
        }
        [HttpPost]
        public IActionResult GetLateEarlyArrivalGroupByDepartmentReport([FromQuery] LateEarlyArrivalGroupByDepartmentReportCritria param)
        {
            var response = _reportGeneratorBL.GenerateLateEarlyArrivalGroupByDepartmentReport(param);
            if (response != null && response.IsSuccessStatusCode)
            {
                var contentStream = response.Content.ReadAsStream();
                switch (param.ExportFormat)
                {
                    case ExportFormat.Pdf:
                        return File(contentStream, "application/pdf", "LateEarlyArrivalGroupByDepartmentReport.pdf");
                    case ExportFormat.Excel:
                        // Return Excel file
                        // return File(contentStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "CompaniesReport.xlsx");
                        break;
                    // Handle other export types as needed
                    default:
                        // Handle unsupported export types
                        break;
                }
            }
            return NotFound();
        }
    }
}
