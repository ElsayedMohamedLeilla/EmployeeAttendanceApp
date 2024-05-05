namespace Dawem.Models.Response.Dawem.Attendances
{
    public class GetEmployeeAttendanceInPeriodReportModel
    {
        public string EmployeeName { get; set; }
        public string JobTitleName { get; set; }
        public DateTime AttendanceDate { get; set; }
        public DateTime LeaveDate { get; set; }
        public decimal TotalWorkingHours { get; set; }
        public decimal TotalLateArrivalsHours { get; set; }
        public decimal TotalEarlyDeparturesHours { get; set; }
        public decimal TotalOverTimeHours { get; set; }
    }
}
