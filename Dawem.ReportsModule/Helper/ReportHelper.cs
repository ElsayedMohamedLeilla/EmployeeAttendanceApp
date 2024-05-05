using FastReport;
using FastReport.Export.OoXML;
using FastReport.Export.Pdf;
using FastReport.Export.Image;
using FastReport.Utils;


namespace Dawem.ReportsModule.Helper
{
    public class ReportHelper
    {
        private byte[] ExportToPdf(FastReport.Report report)
        {
            using MemoryStream stream = new();
            PDFExport export = new PDFExport();
            report.Export(export, stream);
            return stream.ToArray();
        }

        private byte[] ExportToExcel(FastReport.Report report)
        {
            using MemoryStream stream = new();
            Excel2007Export export = new Excel2007Export();
            report.Export(export, stream);
            return stream.ToArray();
        }
        private byte[] ExportToImage(FastReport.Report report)
        {
            using MemoryStream stream = new();
            ImageExport export = new ImageExport();
            export.ImageFormat = ImageExportFormat.Png; // Specify the desired image format
            report.Export(export, stream);
            return stream.ToArray();
        }
        //private void ShowReportInViewer(FastReport.Report report)
        //{
        //    // Create a new report viewer form
        //    using ReportViewerForm viewerForm = new();
        //    // Attach the report to the viewer
        //    viewerForm.Report = report;

        //    // Show the report viewer form
        //    viewerForm.ShowDialog();
        //}
    }
}
