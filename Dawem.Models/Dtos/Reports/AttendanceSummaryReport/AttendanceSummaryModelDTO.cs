namespace Dawem.Models.Dtos.Reports.AttendanceSummaryReport
{
    public class AttendanceSummaryModelDTO
    {

        public int EmployeeId { get; set; }

        public int EmployeeNumber { get; set; }
        public string EmployeeName { get; set; }
        public double Total_Working_Hours { get; set; }
        public double Total_Absences { get; set; }
        public double Total_Late_Arrivals { get; set; }
        public double Total_Early_Departures { get; set; }
    }
}
