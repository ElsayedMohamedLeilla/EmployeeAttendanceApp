using Dawem.Contract.BusinessLogicCore.Dawem;
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

        public static HttpResponseMessage ExportToPdf(Report report)
        {

            if (report.Prepare())
            {
                MemoryStream stream = new();
                PDFSimpleExport pdfExport = new();
                report.Export(pdfExport, stream);
                stream.Position = 0;
                HttpResponseMessage response = new(HttpStatusCode.OK);
                response.Content = new StreamContent(stream);
                response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/pdf");
                response.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("inline");
                //response.Content.Headers.ContentDisposition.FileName = exporterModelDTO.ReportName;
                report.Dispose();
                return response;
            }
            else
            {
                report.Dispose();
                throw new BusinessValidationException(AmgadKeys.SorryErrorHappendDuringExtractingReport);
            }
        }
       


    }
}
