using Dawem.Enums.Generals;

namespace Dawem.Models.Response.Attendances
{
    public class GetEmployeeAttendanceInfoDTO
    {
        // will implement
        public string EmployeeName { get; set; }
        public int EmployeeAttendanceId { get; set; }

        public string CheckInTime { get; set; }
        public string CheckOutTime { get; set; }
        public DateTime LocalDate { get; set; }
        public EmployeeStatus EmployeeStatus { get; set; }
    }
}
