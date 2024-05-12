namespace Dawem.Models.DTOs.Dawem.Reports.AttendanceReports
{
    public class GetEmployeeAttendanceReportInAperiodModel
    {
        public string EmployeeName { get; set; }
        public string JobTitleName { get; set; }
        public string Day { get; set; }
        public int RowIndex { get; set; }
        public DateTime AttendanceDate { get; set; }
        public DateTime LeaveDate { get; set; }
        public int TotalWorkingHours { get; set; }
        public int TotalLateArrivalsHours { get; set; }
        public int TotalEarlyDeparturesHours { get; set; }
        public int TotalOverTimeHours { get; set; }



    }
}
