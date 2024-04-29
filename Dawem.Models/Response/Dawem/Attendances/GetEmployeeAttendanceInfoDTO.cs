namespace Dawem.Models.Response.Dawem.Attendances
{
    public class GetEmployeeAttendanceInfoDTO
    {
        public string EmployeeName { get; set; }
        public DateTime Date { get; set; }
        public string CheckInDateTime { get; set; }
        public string CheckOutDateTime { get; set; }
        public string Status { get; set; }
        public string LateArrivals { get; set; }
        public string EarlyDepartures { get; set; }
        public string WorkingHours { get; set; }
        public string OverTime { get; set; }
        public string ZoneName { get; set; }
        public List<GetEmployeeAttendanceInfoFingerprintDTO> Fingerprints { get; set; }

    }
}
