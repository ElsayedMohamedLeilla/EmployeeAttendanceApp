using Dawem.Enums.Generals;
using Dawem.Models.Dtos.Dawem.Reports.ExporterModel;
using Dawem.Models.Response.Dawem.Attendances;
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
                #region Set Paremetes

                #region General Parameters
                if (exporterModelDTO.ReportType == ReportType.EmployeeDailyAttendanceGroupByDayReport
                    || exporterModelDTO.ReportType == ReportType.AttendaceLeaveStatusByDepartmentIDReport 
                    || exporterModelDTO.ReportType == ReportType.AttendaceLeaveStatusByEmployeeIDReport)
                {
                    report.SetParameterValue("DateFrom", param.DateFrom != default ? param.DateFrom.Date.ToShortDateString() : null);
                    report.SetParameterValue("DateTo", param.DateTo != default ? param.DateTo.Date.ToShortDateString() : null);
                    report.SetParameterValue("CompanyID", exporterModelDTO.CompanyID );
                    report.SetParameterValue("EmployeeID", param.EmployeeID ?? 0);
                    report.SetParameterValue("DepartmentId", param.DepartmentId ?? 0);
                    report.SetParameterValue("CompanyName", exporterModelDTO.CompanyName);
                }
                #endregion

                if (exporterModelDTO.ReportType == ReportType.EmployeeDailyAttendanceGroupByDayReport)
                {
                    report.SetParameterValue("ZoneId", param.ZoneId);
                    report.SetParameterValue("ZoneName", param.ZoneName == null ? "كل المناطق" : param.ZoneName);
                    report.SetParameterValue("DepartmentName", param.DepartmentName == null ? "كل الاقسام" : param.DepartmentName);
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
                    // Report preparation failed
                    // Handle appropriately
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


    }
}
