namespace Dawem.Models.Response.Dawem.Attendances
{
    public class GetEmployeeAttendancesResponseForWebAdminModelDTO
    {
        public int Id { get; set; } //employeeAteendance Id
        public int EmployeeNumber { get; set; }
        public string EmployeeName { get; set; }
        public DateTime Date { get; set; } // attendance Date
        public string CheckInTime { get; set; }
        public string CheckOutTime { get; set; }
        public string Status { get; set; }
        public string WayOfRecognition { get; set; }
    }
}
