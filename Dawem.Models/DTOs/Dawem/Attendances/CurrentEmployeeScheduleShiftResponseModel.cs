using Dawem.Enums.Generals;

namespace Dawem.Models.Dtos.Dawem.Attendances
{
    public class CurrentEmployeeScheduleShiftResponseModel
    {
        public WeekDay WeekDay { get; set; }
        public bool IsVacation { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
    }
}
