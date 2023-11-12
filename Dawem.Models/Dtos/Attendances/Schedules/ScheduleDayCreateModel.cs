using Dawem.Enums.Generals;

namespace Dawem.Models.Dtos.Attendances.Schedules
{
    public class ScheduleDayCreateModel
    {
        public WeekDay WeekDay { get; set; }
        public int? ShiftId { get; set; }
    }
}
