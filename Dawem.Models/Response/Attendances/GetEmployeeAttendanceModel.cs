using Dawem.Enums.Generals;

namespace Dawem.Models.Response.Schedules.Schedules
{
    public class GetEmployeeAttendanceModel
    {
        public int Id { get; set; }
        public EmployeeAttendanceStatus AttendanceStatus { get; set; }
        public EmployeeAttendanceStatus CheckInStatus { get; set; }
        public EmployeeAttendanceStatus CheckOutStatus { get; set; }
        public int Day { get; set; }
        public WeekDay WeekDay { get; set; }
        public string WeekDayName { get; set; }
        public TimeOnly CheckInTime { get; set; }
        public TimeOnly? CheckOutTime { get; set; }
        public TimeOnly? TotalTime { get; set; }
    }
}
