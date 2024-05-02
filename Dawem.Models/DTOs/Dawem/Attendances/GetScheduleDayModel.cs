using Dawem.Enums.Generals;
using Dawem.Models.Dtos.Dawem.Schedules.Schedules;

namespace Dawem.Models.Dtos.Dawem.Attendances
{
    public class GetScheduleDayModel
    {
        public int Id { get; set; }
        public WeekDay WeekDay { get; set; }
        public int? ShiftId { get; set; }
        public ShiftInfoModel ShiftInfo { get; set; }
    }
}
