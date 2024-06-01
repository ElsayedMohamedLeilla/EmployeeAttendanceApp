using Dawem.Contract.BusinessLogic.Dawem.Reports;
using Dawem.Enums.Generals;
using Dawem.Models.Response.Dawem.Attendances;
using Dawem.Translations;
using Microsoft.AspNetCore.Mvc;

namespace Dawem.API.Areas.Dawem.Controllers.Reports
{
    [Route(LeillaKeys.DawemApiControllerAction), ApiController, DawemAuthorize]
    public class ReportController : DawemControllerBase
    {
        private readonly IReportGeneratorBL _reportGeneratorBL;
        public ReportController(IReportGeneratorBL reportGeneratorBL)
        {
            _reportGeneratorBL = reportGeneratorBL;
        }

        #region Attendance Report

        [HttpPost]
        public IActionResult GetEmployeeDailyAttendanceGroupByDay([FromQuery]  ReportCritria param)
        {
            var response = _reportGeneratorBL.GenerateEmployeeDailyAttendanceGroupByDay(param);
            if (response != null && response.IsSuccessStatusCode)
            {
                var contentStream = response.Content.ReadAsStream();
                switch (param.ExportFormat)
                {
                    case ExportFormat.Pdf:
                        return File(contentStream, "application/pdf", "EmployeeDailyAttendanceGroupByDay.pdf");
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
        public IActionResult GetEmployeeAttendanceByDepartmentReport([FromQuery] ReportCritria param)
        {
            var response = _reportGeneratorBL.GenerateAttendaceLeaveStatusByDepartmentID(param);
            if (response != null && response.IsSuccessStatusCode)
            {
                var contentStream = response.Content.ReadAsStream();
                switch (param.ExportFormat)
                {
                    case ExportFormat.Pdf:
                        return File(contentStream, "application/pdf", "EmployeeAttendanceByDepartmentReport.pdf");
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
        public IActionResult GetAttendaceLeaveStatusShortGroupByJobReport([FromQuery] ReportCritria param)
        {
            var response = _reportGeneratorBL.GenerateAttendaceLeaveStatusShortGroupByJobReport(param);
            if (response != null && response.IsSuccessStatusCode)
            {
                var contentStream = response.Content.ReadAsStream();
                switch (param.ExportFormat)
                {
                    case ExportFormat.Pdf:
                        return File(contentStream, "application/pdf", "AttendaceLeaveStatusShortGroupByJobReport.pdf");
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
        public IActionResult GetAttendanceDetailsByEmployeeIDReport([FromQuery] ReportCritria param)
        {
            var response = _reportGeneratorBL.GenerateAttendanceDetailsByEmployeeIDReport(param);
            if (response != null && response.IsSuccessStatusCode)
            {
                var contentStream = response.Content.ReadAsStream();
                switch (param.ExportFormat)
                {
                    case ExportFormat.Pdf:
                        return File(contentStream, "application/pdf", "AttendanceDetailsByEmployeeIDReport.pdf");
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
        public IActionResult GetLateEarlyArrivalGroupByDepartmentReport([FromQuery] ReportCritria param)
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
        [HttpPost]
        public IActionResult GetLateEarlyArrivalGroupByEmployeeReport([FromQuery] ReportCritria param)
        {
            var response = _reportGeneratorBL.GenerateLateEarlyArrivalGroupByEmployeeReport(param);
            if (response != null && response.IsSuccessStatusCode)
            {
                var contentStream = response.Content.ReadAsStream();
                switch (param.ExportFormat)
                {
                    case ExportFormat.Pdf:
                        return File(contentStream, "application/pdf", "LateEarlyArrivalGroupByEmployeeReport.pdf");
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
        public IActionResult GetEmployeeAbsenseInPeriodGroupByEmployeeReport([FromQuery] ReportCritria param)
        {
            var response = _reportGeneratorBL.GenerateEmployeeAbsenseInPeriodGroupByEmployeeReport(param);
            if (response != null && response.IsSuccessStatusCode)
            {
                var contentStream = response.Content.ReadAsStream();
                switch (param.ExportFormat)
                {
                    case ExportFormat.Pdf:
                        return File(contentStream, "application/pdf", "EmployeeAbsenseInPeriodGroupByEmployeeReport.pdf");
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
        public IActionResult GetEmployeeAbsenseInPeriodGroupByDepartmentReport([FromQuery] ReportCritria param)
        {
            var response = _reportGeneratorBL.GenerateEmployeeAbsenseInPeriodGroupByDepartmentReport(param);
            if (response != null && response.IsSuccessStatusCode)
            {
                var contentStream = response.Content.ReadAsStream();
                switch (param.ExportFormat)
                {
                    case ExportFormat.Pdf:
                        return File(contentStream, "application/pdf", "EmployeeAbsenseInPeriodGroupByDepartmentReport.pdf");
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
        public IActionResult GetOverTimeInSelectedPeriodReport([FromQuery] ReportCritria param)
        {
            var response = _reportGeneratorBL.GenerateOverTimeInSelectedPeriodReport(param);
            if (response != null && response.IsSuccessStatusCode)
            {
                var contentStream = response.Content.ReadAsStream();
                switch (param.ExportFormat)
                {
                    case ExportFormat.Pdf:
                        return File(contentStream, "application/pdf", "OverTimeInSelectedPeriodReport.pdf");
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
        public IActionResult GetAttendaceLeaveSummaryReport([FromQuery] ReportCritria param)
        {
            var response = _reportGeneratorBL.GenerateAttendaceLeaveSummaryReport(param);
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
        #endregion


        #region Summons Report
        [HttpPost]
        public IActionResult GetBriefingSummonsInPeriodReport([FromQuery] ReportCritria param)
        {
            var response = _reportGeneratorBL.GenerateBriefingSummonsInPeriodReport(param);
            if (response != null && response.IsSuccessStatusCode)
            {
                var contentStream = response.Content.ReadAsStream();
                switch (param.ExportFormat)
                {
                    case ExportFormat.Pdf:
                        return File(contentStream, "application/pdf", "BriefingSummonsInPeriodReport.pdf");
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
        public IActionResult GetGetSummonsDetailsInPeriodReport([FromQuery] ReportCritria param)
        {
            var response = _reportGeneratorBL.GenerateSummonsDetailsInPeriodReport(param);
            if (response != null && response.IsSuccessStatusCode)
            {
                var contentStream = response.Content.ReadAsStream();
                switch (param.ExportFormat)
                {
                    case ExportFormat.Pdf:
                        return File(contentStream, "application/pdf", "SummonsDetailsInPeriodReport.pdf");
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
        #endregion

    }
}
