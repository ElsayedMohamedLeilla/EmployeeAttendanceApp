using Dawem.Contract.BusinessLogic.Dawem.Reports;
using Dawem.Enums.Generals;
using Dawem.Models.Response.Dawem.Attendances;
using Dawem.Translations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dawem.API.Areas.Dawem.Controllers.Reports
{
    [Route(LeillaKeys.DawemApiControllerAction),
        ApiController,
        Authorize, DawemAuthorize
        ]
    public class ReportController : DawemControllerBase
    {
        private readonly IReportGeneratorBL _reportGeneratorBL;
        public ReportController(IReportGeneratorBL reportGeneratorBL)
        {
            _reportGeneratorBL = reportGeneratorBL;
        }
        [HttpPost]
        public IActionResult GetAttendanceForAllEmployeeReport([FromQuery]  GetEmployeeAttendanceInPeriodReportParameters param)
        {
            var response = _reportGeneratorBL.GenerateAttendanceForAllEmployeeReport(param);
            if (response != null && response.IsSuccessStatusCode)
            {
                var contentStream = response.Content.ReadAsStream();
                switch (param.ExportFormat)
                {
                    case ExportFormat.Pdf:
                        return File(contentStream, "application/pdf", "TotalAttendanceReportForAllEmployee.pdf");
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
            return BadRequest();
        }
        [HttpPost]
        public IActionResult GetEmployeeAttendanceByDepartmentReport([FromQuery] GetEmployeeAttendanceInPeriodReportParameters param)
        {
            var response = _reportGeneratorBL.GenerateAttendaceLeaveStatusByDepartmentID(param);
            if (response != null && response.IsSuccessStatusCode)
            {
                var contentStream = response.Content.ReadAsStream();
                switch (param.ExportFormat)
                {
                    case ExportFormat.Pdf:
                        return File(contentStream, "application/pdf", "EmployeeAttendanceByDepartment.pdf");
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
        [HttpPost]
        public IActionResult GetEmployeeAttendanceByEmployeeReport([FromQuery]  GetEmployeeAttendanceInPeriodReportParameters param)
        {
            var response = _reportGeneratorBL.GenerateAttendaceLeaveStatusByEmployeeID(param);
            if (response != null && response.IsSuccessStatusCode)
            {
                var contentStream = response.Content.ReadAsStream();
                switch (param.ExportFormat)
                {
                    case ExportFormat.Pdf:
                        return File(contentStream, "application/pdf", "EmployeeAttendanceByEmployee.pdf");
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
        [HttpPost]
        public IActionResult GetAttendaceLeaveSummaryReport([FromQuery] GetEmployeeAttendanceInPeriodReportParameters param)
        {
            var response = _reportGeneratorBL.GenerateAttendaceLeaveSummary(param);
            if (response != null && response.IsSuccessStatusCode)
            {
                var contentStream = response.Content.ReadAsStream();
                switch (param.ExportFormat)
                {
                    case ExportFormat.Pdf:
                        return File(contentStream, "application/pdf", "AttendaceLeaveSummaryReport.pdf");
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
