namespace Dawem.Models.Response.Dawem.Attendances
{
    public class GetEmployeeAttendanceInfoDTO
    {
        public string EmployeeName { get; set; }
        public DateTime Date { get; set; }
        public string CheckInTime { get; set; }
        public string CheckOutTime { get; set; }
        public string Status { get; set; }
        public string TimeGap { get; set; }
        public string ZoneName { get; set; }
        public List<GetEmployeeAttendanceInfoFingerprintDTO> Fingerprints { get; set; }

    }
}
