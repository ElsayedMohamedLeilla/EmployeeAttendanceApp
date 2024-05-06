namespace Dawem.Models.Dtos.Dawem.Schedules.Schedules
{
    public class ShiftInfoModel
    {
        public int ShiftId { get; set; }
        public TimeSpan ShiftCheckInTime { get; set; }
        public TimeSpan ShiftCheckOutTime { get; set; }
        public int ShiftAllowedMinutes { get; set; }
    }
}
