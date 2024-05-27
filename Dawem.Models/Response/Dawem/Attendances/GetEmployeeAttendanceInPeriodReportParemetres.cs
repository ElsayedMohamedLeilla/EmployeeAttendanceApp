using Dawem.Enums.Generals;

namespace Dawem.Models.Response.Dawem.Attendances
{
    public class ReportCritria
    {
        public int? EmployeeID { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public int? DepartmentId { get; set; }
        public int? ZoneId { get; set; }
        public int? JobTitleID { get; set; }
        public ExportFormat ExportFormat { get; set; }
        public int? OverTimeFrom { get; set; }
        public int? OverTimeTo { get; set; }
        public bool? WithoutPermision { get; set; }






    }
}
