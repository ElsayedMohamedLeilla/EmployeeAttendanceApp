using Dawem.Enums.Generals;

namespace Dawem.Models.Dtos.Attendances.Schedules
{
    public class ScheduleDayUpdateModel
    {
        public int Id { get; set; }
        public WeekDay WeekDay { get; set; }
        public int? ShiftId { get; set; }
    }
}
