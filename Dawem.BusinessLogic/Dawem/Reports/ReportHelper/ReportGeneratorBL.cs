using Dawem.Contract.BusinessLogic.Dawem.Reports;
using Dawem.Contract.Repository.Manager;
using Dawem.Data;
using Dawem.Enums.Generals;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Dawem.Reports.ExporterModel;
using Dawem.Models.Response.Dawem.Attendances;
using Dawem.ReportsModule.Helper;
using Dawem.Translations;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
namespace Dawem.BusinessLogic.Dawem.Reports.ReportHelper
{
    public class ReportGeneratorBL : IReportGeneratorBL
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IConfiguration _configuration;
        private readonly RequestInfo _requestInfo;
        private readonly IRepositoryManager repositoryManager;
        //private readonly ApplicationDBContext context;


        public ReportGeneratorBL(ApplicationDBContext _context, IRepositoryManager _repositoryManager, IWebHostEnvironment hostingEnvironment, IConfiguration configuration, RequestInfo requestInfo)
        {
            _hostingEnvironment = hostingEnvironment;
            _configuration = configuration;
            _requestInfo = requestInfo;
            repositoryManager = _repositoryManager;
            //_context = context;

        }

        public HttpResponseMessage GenerateEmployeeDailyAttendanceGroupByDay(GetEmployeeAttendanceInPeriodReportParameters param)
        {
            //object[] parameters =
            //{
            //     new SqlParameter("@EmployeeID", param.EmployeeID ?? 0),
            //     new SqlParameter("@DateFrom", param.DateFrom),
            //     new SqlParameter("@DateTo", param.DateTo),
            //     new SqlParameter("@DepartmentID", param.DepartmentId ?? 0),
            //     new SqlParameter("@ZoneID", param.ZoneId ?? 0),
            //     new SqlParameter("@JobTitleID", param.JobTitleID ?? 0),
            //     new SqlParameter("@CompanyID", _requestInfo.CompanyId),

            // };
            //var dataSource = GetDataSource(parameters, ReportType.EmployeeDailyAttendanceGroupByDayReport);
            ExporterModelDTO exporterModelDTO = new()
            {
                FolderName = AmgadKeys.AttendanceReports,
                ReportType = ReportType.EmployeeDailyAttendanceGroupByDayReport,
                //DataSource = dataSource
            };

            return GenerateReport(exporterModelDTO, param);

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
        public HttpResponseMessage GenerateAttendaceLeaveStatusShortGroupByJobReport(GetEmployeeAttendanceInPeriodReportParameters param)
        {
            ExporterModelDTO exporterModelDTO = new()
            {
                FolderName = AmgadKeys.AttendanceReports,
                ReportType = ReportType.AttendaceLeaveStatusShortGroupByJobReport,
            };
            return GenerateReport(exporterModelDTO, param);
        }
        public HttpResponseMessage GenerateAttendanceDetailsByEmployeeIDReport(GetEmployeeAttendanceInPeriodReportParameters param)
        {
            ExporterModelDTO exporterModelDTO = new()
            {
                FolderName = AmgadKeys.AttendanceReports,
                ReportType = ReportType.AttendanceDetailsByEmployeeIDReport,
            };
            return GenerateReport(exporterModelDTO, param);
        }

        public HttpResponseMessage GenerateLateEarlyArrivalGroupByDepartmentReport(GetEmployeeAttendanceInPeriodReportParameters param)
        {
            ExporterModelDTO exporterModelDTO = new()
            {
                FolderName = AmgadKeys.AttendanceReports,
                ReportType = ReportType.LateEarlyArrivalGroupByDepartmentReport,
            };
            param.EmployeeID = 0;
            return GenerateReport(exporterModelDTO, param);
        }

        public HttpResponseMessage GenerateLateEarlyArrivalGroupByEmployeeReport(GetEmployeeAttendanceInPeriodReportParameters param)
        {
            ExporterModelDTO exporterModelDTO = new()
            {
                FolderName = AmgadKeys.AttendanceReports,
                ReportType = ReportType.LateEarlyArrivalGroupByEmployeeReport,
            };
            return GenerateReport(exporterModelDTO, param);
        }
        public HttpResponseMessage GenerateEmployeeVacanciesInPeriodReport(GetEmployeeAttendanceInPeriodReportParameters param)
        {
            ExporterModelDTO exporterModelDTO = new()
            {
                FolderName = AmgadKeys.AttendanceReports,
                ReportType = ReportType.EmployeeVacanciesInPeriodReport,
            };
            param.EmployeeID = 0;
            return GenerateReport(exporterModelDTO, param);
        }
        public HttpResponseMessage GenerateReport(ExporterModelDTO exporterModelDTO, GetEmployeeAttendanceInPeriodReportParameters param)
        {
            exporterModelDTO.ReportName = param.ExportFormat == ExportFormat.Pdf ? exporterModelDTO.ReportType.ToString() + AmgadKeys.Pdf :
                             param.ExportFormat == ExportFormat.Excel ? exporterModelDTO.ReportType.ToString() + AmgadKeys.Xlsx :
                             "";
            exporterModelDTO.CompanyID = _requestInfo.CompanyId;
            var ComObj = repositoryManager.CompanyRepository.Get(c => c.Id == _requestInfo.CompanyId).Select(com =>
            new
            {
                com.Name,
                com.Email,
                com.Country.NameAr
            }).FirstOrDefault();
            exporterModelDTO.CompanyName = ComObj.Name;
            exporterModelDTO.CompanyEmail = ComObj.Email ?? "No Email";
            exporterModelDTO.CountryName = ComObj.NameAr;


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
        public IEnumerable<dynamic> GetDataSource(object[] parameters, ReportType reportType)
        {
            IEnumerable<dynamic> result = null;
            using (ApplicationDBContext context = new ApplicationDBContext())
            {
                if (reportType == ReportType.EmployeeDailyAttendanceGroupByDayReport)
                {
                    string SqlQuery = @"
                                       EXEC [dbo].EmployeeDailyAttendanceGroupByDayReport 
                                       @EmployeeID = @EmployeeID,
                                       @DateFrom = @DateFrom,
                                       @DateTo = @DateTo,
                                       @DepartmentID = @DepartmentID,
                                       @ZoneID = @ZoneID,
                                       @JobTitleID = @JobTitleID,
                                       @CompanyID = @CompanyID"; context.Database.SetCommandTimeout(3600);
                   // result = context.EmployeeDailyAttendanceGroupByDayReport.FromSqlRaw(SqlQuery, parameters).ToList();
                }
            }



            return result;
        }

       
    }
}
