using Dawem.Enums.Generals;

namespace Dawem.Models.Dtos.Employees.Attendance
{
    public class ScheduleDayCreateModel
    {
        public WeekDay WeekDay { get; set; }
        public int? ShiftId { get; set; }
    }
}
