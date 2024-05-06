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
        public HttpResponseMessage GenerateAttendanceForAllEmployeeReport(GetEmployeeAttendanceInPeriodReportParameters param)
        {
            string connectionString = _configuration.GetConnectionString(LeillaKeys.DawemConnectionString);
            string webRootPath = _hostingEnvironment.WebRootPath;
            string reportPath = Path.Combine(webRootPath, "uploads", "Reports", "AttendanceReports", "AttendanceForAllEmployeeReport.frx");
            HttpResponseMessage response = null;
            ExporterModelDTO exporterModelDTO = new()
            {
                ConnectionString = connectionString,
                Path = reportPath,
                ReportName = param.ExportFormat == ExportFormat.Pdf ? "EmployeeAttendanceSummaryReport.pdf" :
                             param.ExportFormat == ExportFormat.Excel ? "EmployeeAttendanceSummaryReport.xlsx" :
                             "",
                CompanyID = _requestInfo.CompanyId,
                ReportType = ReportType.AttendanceForAllEmployeeReport

            };
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
        public HttpResponseMessage GenerateAttendaceLeaveStatusByDepartmentID(GetEmployeeAttendanceInPeriodReportParameters param)
        {
            string connectionString = _configuration.GetConnectionString(LeillaKeys.DawemConnectionString);
            string webRootPath = _hostingEnvironment.WebRootPath;
            string reportPath = Path.Combine(webRootPath, "uploads", "Reports", "AttendanceReports", "AttendaceLeaveStatusByDepartmentIDReport.frx");
            HttpResponseMessage response = null;
            ExporterModelDTO exporterModelDTO = new()
            {
                ConnectionString = connectionString,
                Path = reportPath,
                ReportName = param.ExportFormat == ExportFormat.Pdf ? "AttendaceLeaveStatusByDepartmentIDReport.pdf" :
                             param.ExportFormat == ExportFormat.Excel ? "AttendaceLeaveStatusByDepartmentIDReport.xlsx" :
                             "",
                CompanyID = _requestInfo.CompanyId,
                ReportType = ReportType.AttendaceLeaveStatusByDepartmentIDReport
            };
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
        public HttpResponseMessage GenerateAttendaceLeaveStatusByEmployeeID(GetEmployeeAttendanceInPeriodReportParameters param)
        {
            string connectionString = _configuration.GetConnectionString(LeillaKeys.DawemConnectionString);
            string webRootPath = _hostingEnvironment.WebRootPath;
            string reportPath = Path.Combine(webRootPath, "uploads", "Reports", "AttendanceReports", "AttendaceLeaveStatusByEmployeeIDReport.frx");
            HttpResponseMessage response = null;
            ExporterModelDTO exporterModelDTO = new()
            {
                ConnectionString = connectionString,
                Path = reportPath,
                ReportName = param.ExportFormat == ExportFormat.Pdf ? "AttendaceLeaveStatusByEmployeeIDReport.pdf" :
                             param.ExportFormat == ExportFormat.Excel ? "AttendaceLeaveStatusByEmployeeIDReport.xlsx" :
                             "",
                CompanyID = _requestInfo.CompanyId,
                ReportType = ReportType.AttendaceLeaveStatusByEmployeeIDReport
            };
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

        public HttpResponseMessage GenerateAttendaceLeaveSummary(GetEmployeeAttendanceInPeriodReportParameters param)
        {
            string connectionString = _configuration.GetConnectionString(LeillaKeys.DawemConnectionString);
            string webRootPath = _hostingEnvironment.WebRootPath;
            string reportPath = Path.Combine(webRootPath, "uploads", "Reports", "AttendanceReports", "AttendaceLeaveSummaryReport.frx");
            HttpResponseMessage response = null;
            ExporterModelDTO exporterModelDTO = new()
            {
                ConnectionString = connectionString,
                Path = reportPath,
                ReportName = param.ExportFormat == ExportFormat.Pdf ? "AttendaceLeaveSummaryReport.pdf" :
                             param.ExportFormat == ExportFormat.Excel ? "AttendaceLeaveSummaryReport.xlsx" :
                             "",
                CompanyID = _requestInfo.CompanyId,
                ReportType = ReportType.AttendaceLeaveSummaryReport
            };
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
