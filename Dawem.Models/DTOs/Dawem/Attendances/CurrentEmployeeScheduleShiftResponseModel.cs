using Dawem.Enums.Generals;

namespace Dawem.Models.Dtos.Dawem.Attendances
{
    public class CurrentEmployeeScheduleShiftResponseModel
    {
        public WeekDay WeekDay { get; set; }
        public bool IsVacation { get; set; }
        public TimeOnly? StartTime { get; set; }
        public TimeOnly? EndTime { get; set; }
    }
}
