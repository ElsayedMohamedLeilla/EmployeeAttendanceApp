namespace Dawem.Models.Response.Attendances
{
    public class GetEmployeeAttendancesResponseForWebAdminModelDTO
    {
        public int Id { get; set; } //employeeAteendance Id
        public string EmployeeName { get; set; }
        public DateTime Date { get; set; } // ateendance Date
        public string CheckInTime { get; set; }
        public string CheckOutTime { get; set; }
        public string Status { get; set; }
        public double TimeGap { get; set; }
        public string WayOfRecognition { get; set; }
        //public double Latitude { get; set; }
        //public double Longitude { get; set; }

        public string ZoneName { get; set; }





    }
}
