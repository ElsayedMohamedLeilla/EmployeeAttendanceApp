using Azure;
using Dawem.Contract.BusinessLogic.Dawem.Reports;
using Dawem.Contract.Repository.Manager;
using Dawem.Enums.Generals;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Dawem.Reports.ExporterModel;
using Dawem.Models.Response.Dawem.Attendances;
using Dawem.ReportsModule.Helper;
using Dawem.Translations;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace Dawem.BusinessLogic.Dawem.Reports.ReportHelper
{
    public class ReportGeneratorBL : IReportGeneratorBL
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IConfiguration _configuration;
        private readonly RequestInfo _requestInfo;
        private readonly IRepositoryManager repositoryManager;


        public ReportGeneratorBL(IRepositoryManager _repositoryManager, IWebHostEnvironment hostingEnvironment, IConfiguration configuration, RequestInfo requestInfo)
        {
            _hostingEnvironment = hostingEnvironment;
            _configuration = configuration;
            _requestInfo = requestInfo;
            repositoryManager = _repositoryManager;
        }

        public HttpResponseMessage GenerateEmployeeDailyAttendanceGroupByDay(GetEmployeeAttendanceInPeriodReportParameters param)
        {
            ExporterModelDTO exporterModelDTO = new()
            {
                FolderName = AmgadKeys.AttendanceReports,
                ReportType = ReportType.EmployeeDailyAttendanceGroupByDayReport,
            };
            return GenerateReport(exporterModelDTO,param);

        }
        public HttpResponseMessage GenerateAttendaceLeaveStatusByDepartmentID(GetEmployeeAttendanceInPeriodReportParameters param)
        {
            ExporterModelDTO exporterModelDTO = new()
            {
                FolderName = AmgadKeys.AttendanceReports,
                ReportType = ReportType.AttendaceLeaveStatusByDepartmentIDReport,
            };
            return GenerateReport(exporterModelDTO, param);
        }
        public HttpResponseMessage GenerateAttendaceLeaveStatusByEmployeeID(GetEmployeeAttendanceInPeriodReportParameters param)
        {
            ExporterModelDTO exporterModelDTO = new()
            {
                FolderName = AmgadKeys.AttendanceReports,
                ReportType = ReportType.AttendaceLeaveStatusByEmployeeIDReport,
            };
            return GenerateReport(exporterModelDTO, param);
        }
        public HttpResponseMessage GenerateAttendaceLeaveSummary(GetEmployeeAttendanceInPeriodReportParameters param)
        {
            ExporterModelDTO exporterModelDTO = new()
            {
                FolderName = AmgadKeys.AttendanceReports,
                ReportType = ReportType.AttendaceLeaveSummaryReport,
            };
            return GenerateReport(exporterModelDTO, param);
        }

        public HttpResponseMessage GenerateReport(ExporterModelDTO exporterModelDTO, GetEmployeeAttendanceInPeriodReportParameters param)
        {
            exporterModelDTO.ReportName = param.ExportFormat == ExportFormat.Pdf ? exporterModelDTO.ReportType.ToString() + AmgadKeys.Pdf :
                             param.ExportFormat == ExportFormat.Excel ? exporterModelDTO.ReportType.ToString() + AmgadKeys.Xlsx :
                             "";
            exporterModelDTO.CompanyID = _requestInfo.CompanyId;
            exporterModelDTO.CompanyName = repositoryManager.CompanyRepository.Get(c => c.Id == _requestInfo.CompanyId).Select(com => com.Name).FirstOrDefault();
            exporterModelDTO.BasePath = "uploads\\Reports";
            string connectionString = _configuration.GetConnectionString(LeillaKeys.DawemConnectionString);
            exporterModelDTO.ConnectionString = connectionString;
            string webRootPath = _hostingEnvironment.WebRootPath;
            string reportPath = Path.Combine(webRootPath, exporterModelDTO.BasePath, exporterModelDTO.FolderName, exporterModelDTO.ReportType.ToString() + AmgadKeys.frx);
            exporterModelDTO.FullPath = reportPath;
            HttpResponseMessage response = null;
            switch (param.ExportFormat)

            {
                case ExportFormat.Pdf:
                    response = ReportExporter.ExportToPdf(exporterModelDTO, param);
                    break;
                case ExportFormat.Excel:
                    // Call method to generate Excel report
                    // response = ReportHelper.GenerateExcelWithConnectionString(reportPath, "CompaniesReport.xlsx", Global.ConnectionStringWork);
                    break;
                // Handle other export types as needed
                default:
                    // Handle unsupported export types
                    break;
            }
            return response;
        }
    }
}
