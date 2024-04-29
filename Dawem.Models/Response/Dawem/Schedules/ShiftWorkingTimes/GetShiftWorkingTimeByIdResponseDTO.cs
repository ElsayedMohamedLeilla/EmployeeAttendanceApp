using Dawem.Enums.Generals;

namespace Dawem.Models.Response.Dawem.Schedules.ShiftWorkingTimes
{
    public class GetShiftWorkingTimeByIdResponseDTO
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public string Name { get; set; }
        public TimeSpan CheckInTime { get; set; }
        public TimeSpan CheckOutTime { get; set; }
        public int AllowedMinutes { get; set; }
        public bool IsActive { get; set; }
        public AmPm TimePeriod { get; set; }

    }
}
