using Dawem.Enums.Generals;

namespace Dawem.Models.Dtos.Dawem.Reports.ExporterModel
{
    public class ExporterModelDTO
    {
        public string ReportName { get; set; }
        public string ConnectionString { get; set; }
        public string FullPath { get; set; }

        public int CompanyID { get; set; }
        public string CompanyName { get; set; }
        public string BasePath { get; set; }
        public string FolderName { get; set; }  
        public ReportType ReportType { get; set; }
       public IEnumerable<dynamic> DataSource { get; set; }
        public string CompanyEmail { get; set; }
        public string CountryName { get; set; }



    }
}
