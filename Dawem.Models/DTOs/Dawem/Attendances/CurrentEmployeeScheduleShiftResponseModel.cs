using Dawem.Enums.Generals;

namespace Dawem.Models.Dtos.Dawem.Attendances
{
    public class CurrentEmployeeScheduleShiftResponseModel
    {
        public WeekDay WeekDay { get; set; }
        public bool IsVacation { get; set; }
        public DateTime? StartDateTime { get; set; }
        public DateTime? EndDateTime { get; set; }
        public int? AllowedMinutes { get; set; }
    }
}
