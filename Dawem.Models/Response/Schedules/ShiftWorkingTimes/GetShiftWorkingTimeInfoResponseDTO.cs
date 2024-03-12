using Dawem.Enums.Generals;

namespace Dawem.Models.Response.Schedules.ShiftWorkingTimes
{
    public class GetShiftWorkingTimeInfoResponseDTO
    {
        public int Code { get; set; }
        public string Name { get; set; }
        public TimeOnly CheckInTime { get; set; }
        public TimeOnly CheckOutTime { get; set; }
        public int AllowedMinutes { get; set; }
        public AmPm TimePeriod { get; set; }
        public bool IsActive { get; set; }
        public int EmployeesCount { get; set; }
    }
}
