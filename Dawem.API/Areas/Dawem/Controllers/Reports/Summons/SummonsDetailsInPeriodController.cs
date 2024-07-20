using Dawem.Contract.BusinessLogic.Dawem.Reports;
using Dawem.Enums.Generals;
using Dawem.Models.Response.Dawem.ReportCritrias;
using Dawem.Translations;
using Microsoft.AspNetCore.Mvc;

namespace Dawem.API.Areas.Dawem.Controllers.Reports
{


    [Route(LeillaKeys.DawemApiControllerAction), ApiController, DawemAuthorize]

    public class SummonsDetailsInPeriodController : DawemControllerBase
    {
        private readonly IReportGeneratorBL _reportGeneratorBL;
        public SummonsDetailsInPeriodController(IReportGeneratorBL reportGeneratorBL)
        {
            _reportGeneratorBL = reportGeneratorBL;
        }
        [HttpPost]
        public IActionResult GetSummonsDetailsInPeriodReport([FromQuery] SummonsDetailsInPeriodReportCritria param)
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
    }
}
