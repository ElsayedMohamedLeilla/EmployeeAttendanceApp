using Dawem.Enums.Generals;
using Dawem.Models.Dtos.Dawem.Reports.ExporterModel;
using Dawem.Models.DTOs.Dawem.Generic.Exceptions;
using Dawem.Models.Response.Dawem.Attendances;
using Dawem.Translations;
using FastReport;
using FastReport.Data;
using FastReport.Export.Image;
//using FastReport.Export.OoXML;
//using FastReport.Export.Pdf;
using FastReport.Export.PdfSimple;
using System.Net;


namespace Dawem.ReportsModule.Helper
{
    public class ReportExporter
    {
        //private byte[] ExportToPdf(Report report)
        //{
        //    using MemoryStream stream = new();
        //    PDFExport export = new();
        //    report.Export(export, stream);
        //    return stream.ToArray();
        //}

        //private byte[] ExportToExcel(Report report)
        //{
        //    using MemoryStream stream = new();
        //    Excel2007Export export = new Excel2007Export();
        //    report.Export(export, stream);
        //    return stream.ToArray();
        //}
        private byte[] ExportToImage(Report report)
        {
            using MemoryStream stream = new();
            ImageExport export = new ImageExport();
            export.ImageFormat = ImageExportFormat.Png; // Specify the desired image format
            report.Export(export, stream);
            return stream.ToArray();
        }

        public static HttpResponseMessage ExportToPdf(ExporterModelDTO exporterModelDTO, ReportCritria param)
        {
            Report report = new();
            try
            {
                MsSqlDataConnection connection = new()
                {
                    ConnectionString = exporterModelDTO.ConnectionString
                };

                report.Dictionary.Connections.Add(connection);
                report.Load(exporterModelDTO.FullPath);

                #region Set Parameters

                SetGeneralParameters(report, param, exporterModelDTO);
                switch (exporterModelDTO.ReportType)
                {
                    case ReportType.EmployeeDailyAttendanceGroupByDayReport:
                        SetEmployeeDailyAttendanceGroupByDayReportParameters(report, param);
                        break;
                    case ReportType.AttendaceLeaveStatusShortGroupByJobReport:
                        SetAttendaceLeaveStatusShortGroupByJobReportParameters(report, param);
                        break;
                    case ReportType.AttendanceDetailsByEmployeeIDReport:
                        SetAttendanceDetailsByEmployeeIDReportParameters(report, param);
                        break;
                    case ReportType.LateEarlyArrivalGroupByDepartmentReport:
                        SetLateEarlyArrivalGroupByDepartmentReportParameters(report, param);
                        break;
                    case ReportType.AttendaceLeaveStatusByDepartmentIDReport:
                        SetAttendaceLeaveStatusByDepartmentIDReportParameters(report, param);
                        break;
                    case ReportType.EmployeeAbsenseInPeriodGroupByEmployeeReport:
                        SetEmployeeAbsenseInPeriodGroupByEmployeeReportParameters(report, param);
                        break;
                    case ReportType.EmployeeAbsenseInPeriodGroupByDepartmentReport:
                        SetEmployeeAbsenseInPeriodGroupByDepartmentReportParameters(report, param);
                        break;
                    case ReportType.OverTimeInSelectedPeriodReport:
                        SetOverTimeInSelectedPeriodReportParameters(report, param);
                        break;
                    case ReportType.AttendaceLeaveSummaryReport:
                        SetAttendaceLeaveSummaryReportParameters(report, param);
                        break;
                    case ReportType.BriefingSummonsInPeriodReport:
                        SetBriefingSummonsInPeriodReportParameters(report, param);
                        break;
                    case ReportType.GetSummonsDetailsInPeriodReport:
                        SetSummonsDetailsInPeriodReportParameters(report, param);
                        break;


                }

                #endregion

                if (report.Prepare())
                {
                    MemoryStream stream = new();
                    PDFSimpleExport pdfExport = new();
                    report.Export(pdfExport, stream);
                    stream.Position = 0;
                    HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
                    response.Content = new StreamContent(stream);
                    response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/pdf");
                    response.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("inline");
                    response.Content.Headers.ContentDisposition.FileName = exporterModelDTO.ReportName;
                    return response;
                }
                else
                {
                    throw new BusinessValidationException(AmgadKeys.SorryErrorHappendDuringExtractingReport);
                }
            }
            catch (Exception ex)
            {
                throw new BusinessValidationException(AmgadKeys.SorryErrorHappendDuringExtractingReport);
            }
            finally
            {
                report.Dispose();
            }

            return new HttpResponseMessage(HttpStatusCode.InternalServerError);
        }
        private static void SetGeneralParameters(Report report, ReportCritria param, ExporterModelDTO exporterModelDTO)
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
        private static void SetEmployeeDailyAttendanceGroupByDayReportParameters(Report report, ReportCritria param)
        {
            report.SetParameterValue("ZoneID", param.ZoneId ?? 0);
            report.SetParameterValue("JobTitleID", param.JobTitleID ?? 0);
        }
        private static void SetAttendaceLeaveStatusShortGroupByJobReportParameters(Report report, ReportCritria param)
        {
            report.SetParameterValue("JobTitleID", param.JobTitleID ?? 0);
        }
        private static void SetAttendaceLeaveStatusByDepartmentIDReportParameters(Report report, ReportCritria param)
        {
            report.SetParameterValue("JobTitleID", param.JobTitleID ?? 0);
        }
        private static void SetAttendanceDetailsByEmployeeIDReportParameters(Report report, ReportCritria param)
        {
            report.SetParameterValue("ZoneID", param.ZoneId ?? 0);
        }
        private static void SetLateEarlyArrivalGroupByDepartmentReportParameters(Report report, ReportCritria param)
        {
            report.SetParameterValue("ZoneID", param.ZoneId ?? 0);
            report.SetParameterValue("JobTitleID", param.JobTitleID ?? 0);
        }
        private static void SetEmployeeAbsenseInPeriodGroupByEmployeeReportParameters(Report report, ReportCritria param)
        {
            report.SetParameterValue("ZoneID", param.ZoneId ?? 0);
            report.SetParameterValue("JobTitleID", param.JobTitleID ?? 0);
            report.SetParameterValue("WithoutPermision", param.WithoutPermision == null ? false : true);
        }
        private static void SetEmployeeAbsenseInPeriodGroupByDepartmentReportParameters(Report report, ReportCritria param)
        {
            report.SetParameterValue("ZoneID", param.ZoneId ?? 0);
            report.SetParameterValue("JobTitleID", param.JobTitleID ?? 0);
            report.SetParameterValue("WithoutPermision", param.WithoutPermision == null ? false : true);
        }

        private static void SetOverTimeInSelectedPeriodReportParameters(Report report, ReportCritria param)
        {
            report.SetParameterValue("JobTitleID", param.JobTitleID ?? 0);
            report.SetParameterValue("ZoneID", param.ZoneId ?? 0);
            report.SetParameterValue("OverTimeFrom", param.OverTimeFrom ?? 0);
            report.SetParameterValue("OverTimeTo", param.OverTimeTo ?? 0);

        }
        private static void SetAttendaceLeaveSummaryReportParameters(Report report, ReportCritria param)
        {

            report.SetParameterValue("JobTitleID", param.JobTitleID ?? 0);
            report.SetParameterValue("ZoneID", param.ZoneId ?? 0);
        }

        private static void SetBriefingSummonsInPeriodReportParameters(Report report, ReportCritria param)
        {

            report.SetParameterValue("JobTitleID", param.JobTitleID ?? 0);
            report.SetParameterValue("ZoneID", param.ZoneId ?? 0);
            report.SetParameterValue("NotifiyWay", param.NotifiyWay);
            report.SetParameterValue("AllowedTimeWithMinutesFrom", param.AllowedTimeWithMinutesFrom);
            report.SetParameterValue("AllowedTimeWithMinutesTo", param.AllowedTimeWithMinutesTo);
            report.SetParameterValue("NoOfRequiredEmployeeFrom", param.NoOfRequiredEmployeeFrom);
            report.SetParameterValue("NoOfRequiredEmployeeTo", param.NoOfRequiredEmployeeTo);
            report.SetParameterValue("PercentageOfDoneFrom", param.PercentageOfDoneFrom);
            report.SetParameterValue("PercentageOfDoneTo", param.PercentageOfDoneTo);
        }

        private static void SetSummonsDetailsInPeriodReportParameters(Report report, ReportCritria param)
        {

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

        }



    }
}
