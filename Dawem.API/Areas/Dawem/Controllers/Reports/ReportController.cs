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
        [HttpPost]
        public IActionResult GetEmployeeDailyAttendanceGroupByDay([FromQuery]  GetEmployeeAttendanceInPeriodReportParameters param)
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
        public IActionResult GetEmployeeAttendanceByDepartmentReport([FromQuery] GetEmployeeAttendanceInPeriodReportParameters param)
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
        public IActionResult GetAttendaceLeaveStatusShortGroupByJobReport([FromQuery] GetEmployeeAttendanceInPeriodReportParameters param)
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
        public IActionResult GetAttendanceDetailsByEmployeeIDReport([FromQuery] GetEmployeeAttendanceInPeriodReportParameters param)
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
        public IActionResult GetLateEarlyArrivalGroupByDepartmentReport([FromQuery] GetEmployeeAttendanceInPeriodReportParameters param)
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
        public IActionResult GetLateEarlyArrivalGroupByEmployeeReport([FromQuery] GetEmployeeAttendanceInPeriodReportParameters param)
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
        public IActionResult GetEmployeeAbsenseInPeriodReport([FromQuery] GetEmployeeAttendanceInPeriodReportParameters param)
        {
            var response = _reportGeneratorBL.GenerateEmployeeAbsenseInPeriodReport(param);
            if (response != null && response.IsSuccessStatusCode)
            {
                var contentStream = response.Content.ReadAsStream();
                switch (param.ExportFormat)
                {
                    case ExportFormat.Pdf:
                        return File(contentStream, "application/pdf", "EmployeeAbsenseInPeriodReport.pdf");
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


        //[HttpPost]
        //public IActionResult GetAttendaceLeaveSummaryReport([FromQuery] GetEmployeeAttendanceInPeriodReportParameters param)
        //{
        //    var response = _reportGeneratorBL.GenerateAttendaceLeaveSummary(param);
        //    if (response != null && response.IsSuccessStatusCode)
        //    {
        //        var contentStream = response.Content.ReadAsStream();
        //        switch (param.ExportFormat)
        //        {
        //            case ExportFormat.Pdf:
        //                return File(contentStream, "application/pdf", "AttendaceLeaveSummaryReport.pdf");
        //            case ExportFormat.Excel:
        //                // Return Excel file
        //                // return File(contentStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "CompaniesReport.xlsx");
        //                break;
        //            // Handle other export types as needed
        //            default:
        //                // Handle unsupported export types
        //                break;
        //        }
        //    }
        //    return NotFound();
        //}



    }
}
