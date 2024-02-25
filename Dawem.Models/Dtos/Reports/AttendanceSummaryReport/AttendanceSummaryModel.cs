using System.Text.Json.Serialization;

namespace Dawem.Models.Dtos.Reports.AttendanceSummaryReport
{
    public class AttendanceSummaryModel
    {
        public int EmployeeId { get; set; }
        public int EmployeeNumber { get; set; }
        public int EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string ShouldAttendCount { get; set; }
        public string ActualAttendCount { get; set; }
        public string VacationsCount { get; set; }
        public string AbsencesCount { get; set; }
        public string WorkingHoursCount { get; set; }
        public string LateArrivalsCount { get; set; }
        public string EarlyDeparturesCount { get; set; }
        public string OverTimeCount { get; set; }
    }
}
