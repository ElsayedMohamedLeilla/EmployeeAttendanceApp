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

        public static HttpResponseMessage ExportToPdf(ExporterModelDTO exporterModelDTO, GetEmployeeAttendanceInPeriodReportParameters param)
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
                        SetEmployeeDailyAttendanceParameters(report, param);
                        break;
                    case ReportType.AttendaceLeaveStatusShortGroupByJobReport:
                        SetLeaveStatusByJobParameters(report, param);
                        break;
                    case ReportType.AttendanceDetailsByEmployeeIDReport:
                        SetAttendanceDetailsParameters(report, param);
                        break;
                    case ReportType.LateEarlyArrivalGroupByDepartmentReport:
                        SetEarlyArrivalGroupByDepartmentParameters(report, param);
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
            }
            finally
            {
                report.Dispose();
            }

            return new HttpResponseMessage(HttpStatusCode.InternalServerError);
        }


        private static void SetGeneralParameters(Report report, GetEmployeeAttendanceInPeriodReportParameters param, ExporterModelDTO exporterModelDTO)
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

        private static void SetEmployeeDailyAttendanceParameters(Report report, GetEmployeeAttendanceInPeriodReportParameters param)
        {
            report.SetParameterValue("ZoneName", param.ZoneName ?? "كل المناطق");
            report.SetParameterValue("DepartmentName", param.DepartmentName ?? "كل الاقسام");
            report.SetParameterValue("ZoneID", param.ZoneId ?? 0);
            report.SetParameterValue("JobTitleID", param.JobTitleID ?? 0);
        }

        private static void SetLeaveStatusByJobParameters(Report report, GetEmployeeAttendanceInPeriodReportParameters param)
        {
            report.SetParameterValue("JobTitleID", param.JobTitleID ?? 0);
        }

        private static void SetAttendanceDetailsParameters(Report report, GetEmployeeAttendanceInPeriodReportParameters param)
        {
            report.SetParameterValue("ZoneID", param.ZoneId ?? 0);
        }

        private static void SetEarlyArrivalGroupByDepartmentParameters(Report report, GetEmployeeAttendanceInPeriodReportParameters param)
        {
            report.SetParameterValue("ZoneID", param.ZoneId ?? 0);
            report.SetParameterValue("JobTitleID", param.JobTitleID ?? 0);
        }


    }
}
