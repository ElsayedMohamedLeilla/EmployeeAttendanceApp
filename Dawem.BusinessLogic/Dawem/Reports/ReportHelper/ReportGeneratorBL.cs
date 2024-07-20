using Dawem.Contract.BusinessLogic.Dawem.Reports;
using Dawem.Contract.BusinessLogicCore.Dawem;
using Dawem.Contract.Repository.Manager;
using Dawem.Data;
using Dawem.Enums.Generals;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Dawem.Reports.ExporterModel;
using Dawem.Models.Response.Dawem.ReportCritrias;
using Dawem.Models.Response.Dawem.ReportCritrias.AttendanceReports;
using Dawem.ReportsModule.Helper;
using Dawem.Translations;
using FastReport;
using FastReport.Data;
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
        private readonly IUploadBLC uploadBLC;
        public ReportGeneratorBL(IUploadBLC _uploadBLC, ApplicationDBContext _context, IRepositoryManager _repositoryManager, IWebHostEnvironment hostingEnvironment, IConfiguration configuration, RequestInfo requestInfo)
        {
            _hostingEnvironment = hostingEnvironment;
            _configuration = configuration;
            _requestInfo = requestInfo;
            repositoryManager = _repositoryManager;
            uploadBLC = _uploadBLC;
        }
       
        #region Attendance Report
        public HttpResponseMessage GenerateEmployeeDailyAttendanceGroupByDay(EmployeeDailyAttendanceGroupByDayReportCritria param)
        {
           
            ExporterModelDTO exporterModelDTO = new()
            {
                FolderName = AmgadKeys.AttendanceReports,
                ReportType = ReportType.EmployeeDailyAttendanceGroupByDayReport,
            };
            
            Report report = GenerateReport(exporterModelDTO, param);
            SetGeneralParameters(report, param, exporterModelDTO);
            report.SetParameterValue("ZoneID", param.ZoneId ?? 0);
            report.SetParameterValue("JobTitleID", param.JobTitleID ?? 0);
            return ExportReport(report, param.ExportFormat);
        }
        public HttpResponseMessage GenerateAttendaceLeaveStatusByDepartmentID(EmployeeAttendanceByDepartmentReportCritria param)
        {
            ExporterModelDTO exporterModelDTO = new()
            {
                FolderName = AmgadKeys.AttendanceReports,
                ReportType = ReportType.AttendaceLeaveStatusByDepartmentIDReport,
            };

            Report report = GenerateReport(exporterModelDTO, param);
            SetGeneralParameters(report, param, exporterModelDTO);
            report.SetParameterValue("JobTitleID", param.JobTitleID ?? 0);
            return ExportReport(report, param.ExportFormat);
        }
        public HttpResponseMessage GenerateAttendaceLeaveStatusShortGroupByJobReport(AttendaceLeaveStatusShortGroupByJobReportCritria param)
        {
            ExporterModelDTO exporterModelDTO = new()
            {
                FolderName = AmgadKeys.AttendanceReports,
                ReportType = ReportType.AttendaceLeaveStatusShortGroupByJobReport,
            };
            Report report = GenerateReport(exporterModelDTO, param);
            SetGeneralParameters(report, param, exporterModelDTO);
            report.SetParameterValue("JobTitleID", param.JobTitleID ?? 0);
            return ExportReport(report, param.ExportFormat);
        }
        public HttpResponseMessage GenerateAttendanceDetailsByEmployeeIDReport(AttendanceDetailsByEmployeeIDReportCritria param)
        {
            ExporterModelDTO exporterModelDTO = new()
            {
                FolderName = AmgadKeys.AttendanceReports,
                ReportType = ReportType.AttendanceDetailsByEmployeeIDReport,
            };
            Report report = GenerateReport(exporterModelDTO, param);
            SetGeneralParameters(report, param, exporterModelDTO);
            report.SetParameterValue("ZoneID", param.ZoneId ?? 0);
            return ExportReport(report, param.ExportFormat);
        }
        public HttpResponseMessage GenerateLateEarlyArrivalGroupByDepartmentReport(LateEarlyArrivalGroupByDepartmentReportCritria param)
        {
            ExporterModelDTO exporterModelDTO = new()
            {
                FolderName = AmgadKeys.AttendanceReports,
                ReportType = ReportType.LateEarlyArrivalGroupByDepartmentReport,
            };
            Report report = GenerateReport(exporterModelDTO, param);
            SetGeneralParameters(report, param, exporterModelDTO);
            report.SetParameterValue("ZoneID", param.ZoneId ?? 0);
            report.SetParameterValue("JobTitleID", param.JobTitleID ?? 0);
            return ExportReport(report, param.ExportFormat);
        }
       
        public HttpResponseMessage GenerateLateEarlyArrivalGroupByEmployeeReport(LateEarlyArrivalGroupByEmployeeReportCritria param)
        {
            ExporterModelDTO exporterModelDTO = new()
            {
                FolderName = AmgadKeys.AttendanceReports,
                ReportType = ReportType.LateEarlyArrivalGroupByEmployeeReport,
            };
            Report report = GenerateReport(exporterModelDTO, param);
            SetGeneralParameters(report, param, exporterModelDTO); 
            return ExportReport(report, param.ExportFormat);
        }
        public HttpResponseMessage GenerateEmployeeAbsenseInPeriodGroupByEmployeeReport(EmployeeAbsenseInPeriodGroupByEmployeeReportCritria param)
        {
            ExporterModelDTO exporterModelDTO = new()
            {
                FolderName = AmgadKeys.AttendanceReports,
                ReportType = ReportType.EmployeeAbsenseInPeriodGroupByEmployeeReport,
            };
            Report report = GenerateReport(exporterModelDTO, param);
            SetGeneralParameters(report, param, exporterModelDTO);
            report.SetParameterValue("ZoneID", param.ZoneId ?? 0);
            report.SetParameterValue("JobTitleID", param.JobTitleID ?? 0);
            report.SetParameterValue("WithoutPermision", param.WithoutPermision == null ? false : true);
            return ExportReport(report, param.ExportFormat);
        }
        
        public HttpResponseMessage GenerateEmployeeAbsenseInPeriodGroupByDepartmentReport(EmployeeAbsenseInPeriodGroupByDepartmentReportCritria param)
        {
            ExporterModelDTO exporterModelDTO = new()
            {
                FolderName = AmgadKeys.AttendanceReports,
                ReportType = ReportType.EmployeeAbsenseInPeriodGroupByDepartmentReport,
            };
            param.EmployeeID = 0; // no filter by employee report by department
            Report report = GenerateReport(exporterModelDTO, param);
            SetGeneralParameters(report, param, exporterModelDTO);
            report.SetParameterValue("ZoneID", param.ZoneId ?? 0);
            report.SetParameterValue("JobTitleID", param.JobTitleID ?? 0);
            report.SetParameterValue("WithoutPermision", param.WithoutPermision == null ? false : true);
            return ExportReport(report, param.ExportFormat);
        }
        
        public HttpResponseMessage GenerateOverTimeInSelectedPeriodReport(OverTimeInSelectedPeriodReportCritria param)
        {
            ExporterModelDTO exporterModelDTO = new()
            {
                FolderName = AmgadKeys.AttendanceReports,
                ReportType = ReportType.OverTimeInSelectedPeriodReport,
            };
            Report report = GenerateReport(exporterModelDTO, param);
            SetGeneralParameters(report, param, exporterModelDTO);
            report.SetParameterValue("JobTitleID", param.JobTitleID ?? 0);
            report.SetParameterValue("ZoneID", param.ZoneId ?? 0);
            report.SetParameterValue("OverTimeFrom", param.OverTimeFrom ?? 0);
            report.SetParameterValue("OverTimeTo", param.OverTimeTo ?? 0);
            return ExportReport(report, param.ExportFormat);
        }
        public HttpResponseMessage GenerateAttendaceLeaveSummaryReport(AttendaceLeaveSummaryReportCritria param)
        {
            ExporterModelDTO exporterModelDTO = new()
            {
                FolderName = AmgadKeys.AttendanceReports,
                ReportType = ReportType.AttendaceLeaveSummaryReport,
            };
            Report report = GenerateReport(exporterModelDTO, param);
            SetGeneralParameters(report, param, exporterModelDTO);
            report.SetParameterValue("JobTitleID", param.JobTitleID ?? 0);
            report.SetParameterValue("ZoneID", param.ZoneId ?? 0);
            return ExportReport(report, param.ExportFormat);
        }

        #endregion

        #region Summon Reports
        public HttpResponseMessage GenerateBriefingSummonsInPeriodReport(BriefingSummonsInPeriodReportCritria param)
        {
            ExporterModelDTO exporterModelDTO = new()
            {
                FolderName = AmgadKeys.SummonReports,
                ReportType = ReportType.BriefingSummonsInPeriodReport,
            };
            Report report = GenerateReport(exporterModelDTO, param);
            SetGeneralParameters(report, param, exporterModelDTO);
            report.SetParameterValue("JobTitleID", param.JobTitleID ?? 0);
            report.SetParameterValue("ZoneID", param.ZoneId ?? 0);
            report.SetParameterValue("NotifiyWay", param.NotifiyWay);
            report.SetParameterValue("AllowedTimeWithMinutesFrom", param.AllowedTimeWithMinutesFrom);
            report.SetParameterValue("AllowedTimeWithMinutesTo", param.AllowedTimeWithMinutesTo);
            report.SetParameterValue("NoOfRequiredEmployeeFrom", param.NoOfRequiredEmployeeFrom);
            report.SetParameterValue("NoOfRequiredEmployeeTo", param.NoOfRequiredEmployeeTo);
            report.SetParameterValue("PercentageOfDoneFrom", param.PercentageOfDoneFrom);
            report.SetParameterValue("PercentageOfDoneTo", param.PercentageOfDoneTo);
            return ExportReport(report, param.ExportFormat);
        }

        public HttpResponseMessage GenerateSummonsDetailsInPeriodReport(SummonsDetailsInPeriodReportCritria param)
        {
            ExporterModelDTO exporterModelDTO = new()
            {
                FolderName = AmgadKeys.SummonReports,
                ReportType = ReportType.SummonsDetailsInPeriodReport,
            };
            Report report = GenerateReport(exporterModelDTO, param);
            SetGeneralParameters(report, param, exporterModelDTO);
            report.SetParameterValue("JobTitleID", param.JobTitleID ?? 0);
            report.SetParameterValue("ZoneID", param.ZoneId ?? 0);
            report.SetParameterValue("NotifiyWay", param.NotifiyWay ?? ReportNotifyWay.All);
            report.SetParameterValue("AllowedTimeWithMinutesFrom", param.AllowedTimeWithMinutesFrom);
            report.SetParameterValue("AllowedTimeWithMinutesTo", param.AllowedTimeWithMinutesTo);
            report.SetParameterValue("NoOfRequiredEmployeeFrom", param.NoOfRequiredEmployeeFrom);
            report.SetParameterValue("NoOfRequiredEmployeeTo", param.NoOfRequiredEmployeeTo);
            report.SetParameterValue("PercentageOfDoneFrom", param.PercentageOfDoneFrom);
            report.SetParameterValue("PercentageOfDoneTo", param.PercentageOfDoneTo);
            report.SetParameterValue("DoneStatus", param.DoneStatus ?? DoneStatus.Both);
            return ExportReport(report, param.ExportFormat);
        }

        public HttpResponseMessage GenerateSummonsDetailsGroupByEmployeeReport(SummonsDetailsGroupByEmployeeReportCritria param)
        {
            ExporterModelDTO exporterModelDTO = new()
            {
                FolderName = AmgadKeys.SummonReports,
                ReportType = ReportType.SummonsDetailsGroupByEmployeeReport,
            };
            Report report = GenerateReport(exporterModelDTO, param);
            SetGeneralParameters(report, param, exporterModelDTO);
            report.SetParameterValue("JobTitleID", param.JobTitleID ?? 0);
            report.SetParameterValue("ZoneID", param.ZoneId ?? 0);
            report.SetParameterValue("NotifiyWay", param.NotifiyWay ?? ReportNotifyWay.All);
            report.SetParameterValue("AllowedTimeWithMinutesFrom", param.AllowedTimeWithMinutesFrom);
            report.SetParameterValue("AllowedTimeWithMinutesTo", param.AllowedTimeWithMinutesTo);
            report.SetParameterValue("DoneStatus", param.DoneStatus ?? DoneStatus.Both);
            return ExportReport(report, param.ExportFormat);
        }
        #endregion

        #region  Statistics
        public HttpResponseMessage GenerateStatisticsOverAperiodReport(StatisticsOverAperiodReportCritria param)
        {
            ExporterModelDTO exporterModelDTO = new()
            {
                FolderName = AmgadKeys.StatisticsReports,
                ReportType = ReportType.StatisticsOverAperiodReport,
            };
            Report report = GenerateReport(exporterModelDTO, param);
            SetGeneralParameters(report, param, exporterModelDTO);
            report.SetParameterValue("JobTitleID", param.JobTitleID ?? 0);
            report.SetParameterValue("ZoneID", param.ZoneId ?? 0);
            report.SetParameterValue("OrderBy", param.statisticsReportOrderBy ?? StatisticsReportOrderBy.Date);
            return ExportReport(report, param.ExportFormat);
        }
        public HttpResponseMessage GenerateStatisticsReportOverAperiodByDepartmentReport(StatisticsReportOverAperiodByDepartmentReportCritria param)
        {
            ExporterModelDTO exporterModelDTO = new()
            {
                FolderName = AmgadKeys.StatisticsReports,
                ReportType = ReportType.StatisticsReportOverAperiodByDepartmentReport,
            };
            Report report = GenerateReport(exporterModelDTO, param);
            SetGeneralParameters(report, param, exporterModelDTO);
            report.SetParameterValue("JobTitleID", param.JobTitleID ?? 0);
            report.SetParameterValue("ZoneID", param.ZoneId ?? 0);
            report.SetParameterValue("OrderBy", param.statisticsReportOrderBy ?? StatisticsReportOrderBy.Date);
            return ExportReport(report, param.ExportFormat);
        }
        public HttpResponseMessage GenerateStatisticsReportOverAperiodGroupByMonthReport(StatisticsReportOverAperiodGroupByMonthReportCritria param)
        {
            ExporterModelDTO exporterModelDTO = new()
            {
                FolderName = AmgadKeys.StatisticsReports,
                ReportType = ReportType.StatisticsReportOverAperiodGroupByMonthReport,
            };
            Report report = GenerateReport(exporterModelDTO, param);
            SetGeneralParameters(report, param, exporterModelDTO);
            report.SetParameterValue("JobTitleID", param.JobTitleID ?? 0);
            report.SetParameterValue("ZoneID", param.ZoneId ?? 0);
            report.SetParameterValue("OrderBy", param.statisticsReportOrderBy ?? StatisticsReportOrderBy.Date);
            return ExportReport(report, param.ExportFormat);
        }
        #endregion

        public Report GenerateReport(ExporterModelDTO exporterModelDTO, BaseReportCritria param)
        {
            #region Fill Main Date
            exporterModelDTO.ReportName = param.ExportFormat == ExportFormat.Pdf ? exporterModelDTO.ReportType.ToString() + AmgadKeys.Pdf :
                             param.ExportFormat == ExportFormat.Excel ? exporterModelDTO.ReportType.ToString() + AmgadKeys.Xlsx :
                             "";
            exporterModelDTO.CompanyID = _requestInfo.CompanyId;
            var ComObj = repositoryManager.CompanyRepository.Get(c => c.Id == _requestInfo.CompanyId).Select(com =>
            new
            {
                com.Name,
                com.Email,
                com.Country.NameAr,
                com.LogoImageName
            }).FirstOrDefault();
            exporterModelDTO.CompanyName = ComObj.Name;
            exporterModelDTO.CompanyEmail = ComObj.Email ?? "No Email";
            exporterModelDTO.CountryName = ComObj.NameAr;
            exporterModelDTO.CompanyLogoPath = uploadBLC.GetFilePath(ComObj.LogoImageName, LeillaKeys.Companies); ;

            exporterModelDTO.BasePath = "uploads\\Reports";
            string connectionString = _configuration.GetConnectionString(LeillaKeys.DawemConnectionString);
            exporterModelDTO.ConnectionString = connectionString;
            string webRootPath = _hostingEnvironment.WebRootPath;
            string reportPath = Path.Combine(webRootPath, exporterModelDTO.BasePath, exporterModelDTO.FolderName, exporterModelDTO.ReportType.ToString() + AmgadKeys.frx);
            exporterModelDTO.FullPath = reportPath;
           
            #endregion

            #region PrepareReport
            Report report = new();
            try
            {
                MsSqlDataConnection connection = new()
                {
                    ConnectionString = exporterModelDTO.ConnectionString
                };
                report.Dictionary.Connections.Add(connection);
                report.Load(exporterModelDTO.FullPath);
                #region Get Company Logo Path
                var picture = report.FindObject(AmgadKeys.ReportCompanyLogo) as PictureObject;
                if (picture != null)
                {
                    string CompanyLogoPath = exporterModelDTO.CompanyLogoPath;
                    if (!string.IsNullOrEmpty(CompanyLogoPath))
                    {
                        picture.ImageLocation = CompanyLogoPath;
                    }
                }
                #endregion
               
            }
            catch (Exception e)
            {

            }

            #endregion

            return report;
           
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


        #region Set Paremetes Methods
        private static void SetGeneralParameters(Report report, BaseReportCritria param, ExporterModelDTO exporterModelDTO)
        {
            report.SetParameterValue("DateFrom", param.DateFrom);
            report.SetParameterValue("DateTo", param.DateTo);
            report.SetParameterValue("EmployeeID", param.EmployeeID ?? 0);
            report.SetParameterValue("DepartmentID", param.DepartmentId ?? 0);
            report.SetParameterValue("CompanyID", exporterModelDTO.CompanyID);
            report.SetParameterValue("CompanyName", exporterModelDTO.CompanyName);
            report.SetParameterValue("DateFromString", param.DateFrom.ToShortDateString());
            report.SetParameterValue("DateToString", param.DateTo.ToShortDateString());
            report.SetParameterValue("CompanyEmail", exporterModelDTO.CompanyEmail);
            report.SetParameterValue("CountryName", exporterModelDTO.CountryName);


        }
        //private static void SetEmployeeDailyAttendanceGroupByDayReportParameters(Report report, BaseReportCritria param)
        //{
        //    report.SetParameterValue("ZoneID", param.ZoneId ?? 0);
        //    report.SetParameterValue("JobTitleID", param.JobTitleID ?? 0);
        //}
        //private static void SetAttendaceLeaveStatusShortGroupByJobReportParameters(Report report, BaseReportCritria param)
        //{
        //    report.SetParameterValue("JobTitleID", param.JobTitleID ?? 0);
        //}
        //private static void SetAttendaceLeaveStatusByDepartmentIDReportParameters(Report report, BaseReportCritria param)
        //{
        //    report.SetParameterValue("JobTitleID", param.JobTitleID ?? 0);
        //}
        //private static void SetAttendanceDetailsByEmployeeIDReportParameters(Report report, BaseReportCritria param)
        //{
        //    report.SetParameterValue("ZoneID", param.ZoneId ?? 0);
        //}
        //private static void SetLateEarlyArrivalGroupByDepartmentReportParameters(Report report, BaseReportCritria param)
        //{
        //    report.SetParameterValue("ZoneID", param.ZoneId ?? 0);
        //    report.SetParameterValue("JobTitleID", param.JobTitleID ?? 0);
        //}
        //private static void SetEmployeeAbsenseInPeriodGroupByEmployeeReportParameters(Report report, BaseReportCritria param)
        //{
        //    report.SetParameterValue("ZoneID", param.ZoneId ?? 0);
        //    report.SetParameterValue("JobTitleID", param.JobTitleID ?? 0);
        //    report.SetParameterValue("WithoutPermision", param.WithoutPermision == null ? false : true);
        //}
        //private static void SetEmployeeAbsenseInPeriodGroupByDepartmentReportParameters(Report report, BaseReportCritria param)
        //{
        //    report.SetParameterValue("ZoneID", param.ZoneId ?? 0);
        //    report.SetParameterValue("JobTitleID", param.JobTitleID ?? 0);
        //    report.SetParameterValue("WithoutPermision", param.WithoutPermision == null ? false : true);
        //}

        //private static void SetOverTimeInSelectedPeriodReportParameters(Report report, BaseReportCritria param)
        //{
        //    report.SetParameterValue("JobTitleID", param.JobTitleID ?? 0);
        //    report.SetParameterValue("ZoneID", param.ZoneId ?? 0);
        //    report.SetParameterValue("OverTimeFrom", param.OverTimeFrom ?? 0);
        //    report.SetParameterValue("OverTimeTo", param.OverTimeTo ?? 0);

        //}
        //private static void SetAttendaceLeaveSummaryReportParameters(Report report, BaseReportCritria param)
        //{

        //    report.SetParameterValue("JobTitleID", param.JobTitleID ?? 0);
        //    report.SetParameterValue("ZoneID", param.ZoneId ?? 0);
        //}

        //private static void SetBriefingSummonsInPeriodReportParameters(Report report, BaseReportCritria param)
        //{

        //    report.SetParameterValue("JobTitleID", param.JobTitleID ?? 0);
        //    report.SetParameterValue("ZoneID", param.ZoneId ?? 0);
        //    report.SetParameterValue("NotifiyWay", param.NotifiyWay);
        //    report.SetParameterValue("AllowedTimeWithMinutesFrom", param.AllowedTimeWithMinutesFrom);
        //    report.SetParameterValue("AllowedTimeWithMinutesTo", param.AllowedTimeWithMinutesTo);
        //    report.SetParameterValue("NoOfRequiredEmployeeFrom", param.NoOfRequiredEmployeeFrom);
        //    report.SetParameterValue("NoOfRequiredEmployeeTo", param.NoOfRequiredEmployeeTo);
        //    report.SetParameterValue("PercentageOfDoneFrom", param.PercentageOfDoneFrom);
        //    report.SetParameterValue("PercentageOfDoneTo", param.PercentageOfDoneTo);
        //}

        //private static void SetSummonsDetailsInPeriodReportParameters(Report report, BaseReportCritria param)
        //{

        //    report.SetParameterValue("JobTitleID", param.JobTitleID ?? 0);
        //    report.SetParameterValue("ZoneID", param.ZoneId ?? 0);
        //    report.SetParameterValue("NotifiyWay", param.NotifiyWay ?? ReportNotifyWay.All);
        //    report.SetParameterValue("AllowedTimeWithMinutesFrom", param.AllowedTimeWithMinutesFrom);
        //    report.SetParameterValue("AllowedTimeWithMinutesTo", param.AllowedTimeWithMinutesTo);
        //    report.SetParameterValue("NoOfRequiredEmployeeFrom", param.NoOfRequiredEmployeeFrom);
        //    report.SetParameterValue("NoOfRequiredEmployeeTo", param.NoOfRequiredEmployeeTo);
        //    report.SetParameterValue("PercentageOfDoneFrom", param.PercentageOfDoneFrom);
        //    report.SetParameterValue("PercentageOfDoneTo", param.PercentageOfDoneTo);
        //    report.SetParameterValue("DoneStatus", param.DoneStatus ?? DoneStatus.Both);

        //}

        //private static void SetSummonsDetailsGroupByEmployeeReportParameters(Report report, BaseReportCritria param)
        //{

        //    report.SetParameterValue("JobTitleID", param.JobTitleID ?? 0);
        //    report.SetParameterValue("ZoneID", param.ZoneId ?? 0);
        //    report.SetParameterValue("NotifiyWay", param.NotifiyWay ?? ReportNotifyWay.All);
        //    report.SetParameterValue("AllowedTimeWithMinutesFrom", param.AllowedTimeWithMinutesFrom);
        //    report.SetParameterValue("AllowedTimeWithMinutesTo", param.AllowedTimeWithMinutesTo);
        //    report.SetParameterValue("DoneStatus", param.DoneStatus ?? DoneStatus.Both);

        //}
        //private static void SetStatisticsOverAperiodReportParameters(Report report, BaseReportCritria param)
        //{

        //    report.SetParameterValue("JobTitleID", param.JobTitleID ?? 0);
        //    report.SetParameterValue("ZoneID", param.ZoneId ?? 0);
        //    report.SetParameterValue("OrderBy", param.statisticsReportOrderBy ?? StatisticsReportOrderBy.Date);

        //}

        #endregion

        public HttpResponseMessage ExportReport(Report report,ExportFormat exportFormat)
        {

            HttpResponseMessage response = null;
            switch (exportFormat)
            {
                case ExportFormat.Pdf:
                    response = ReportExporter.ExportToPdf(report);
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
