using Dawem.Enums.Generals;

namespace Dawem.Models.Dtos.Dawem.Reports.ExporterModel
{
    public class ExporterModelDTO
    {
        public string Path { get; set; }
        public string ReportName { get; set; }
        public string ConnectionString { get; set; }
        public int CompanyID { get; set; }

        public ReportType ReportType { get; set; }
    }
}
