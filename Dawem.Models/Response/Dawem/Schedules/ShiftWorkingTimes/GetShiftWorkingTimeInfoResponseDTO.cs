using Dawem.Enums.Generals;

namespace Dawem.Models.Response.Dawem.Schedules.ShiftWorkingTimes
{
    public class GetShiftWorkingTimeInfoResponseDTO
    {
        public int Code { get; set; }
        public string Name { get; set; }
        public TimeSpan CheckInTime { get; set; }
        public TimeSpan CheckOutTime { get; set; }
        public int AllowedMinutes { get; set; }
        public AmPm TimePeriod { get; set; }
        public bool IsActive { get; set; }
        public int EmployeesCount { get; set; }
    }
}
