using Dawem.Enums.Generals;

namespace Dawem.Models.Response.Dawem.Attendances
{
    public class GetEmployeeAttendanceInPeriodReportParameters
    {
        public int? EmployeeID { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public int? DepartmentId { get; set; }
        public int? ZoneId { get; set; }
        public int? JobTitleID { get; set; }
        public ExportFormat ExportFormat { get; set; }
        public string ZoneName { get; set; }
        public string DepartmentName { get; set; }
        public string JobTitleName { get; set; }
        public bool? WithPermision { get; set; }
        public bool? BothWithandWithoutPermision { get; set; }






    }
}
