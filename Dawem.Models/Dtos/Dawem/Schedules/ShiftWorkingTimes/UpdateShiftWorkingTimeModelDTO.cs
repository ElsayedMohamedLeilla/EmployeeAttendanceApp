using Dawem.Enums.Generals;

namespace Dawem.Models.Dtos.Dawem.Schedules.ShiftWorkingTimes
{
    public class UpdateShiftWorkingTimeModelDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public AmPm TimePeriod { get; set; }
        public TimeOnly CheckInTime { get; set; }
        public TimeOnly CheckOutTime { get; set; }
        public int AllowedMinutes { get; set; }
        public bool IsFreezed { get; set; }
        public bool IsActive { get; set; }


    }
}
