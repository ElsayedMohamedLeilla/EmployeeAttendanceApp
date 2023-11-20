using Dawem.Enums.Generals;

namespace Dawem.Models.Response.Schedules.ShiftWorkingTimes
{
    public class GetShiftWorkingTimeResponseModelDTO
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public AmPm TimePeriod { get; set; }
        public string Name { get; set; }
        public TimeOnly CheckInTime { get; set; }
        public TimeOnly CheckOutTime { get; set; }
        public double AllowedMinutes { get; set; }
        public bool IsActive { get; set; }
    }
}
