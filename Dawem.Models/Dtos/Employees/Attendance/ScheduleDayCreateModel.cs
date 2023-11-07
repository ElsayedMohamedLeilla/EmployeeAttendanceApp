using Dawem.Enums.General;

namespace Dawem.Models.Dtos.Employees.Employees
{
    public class ScheduleDayCreateModel
    {
        public WeekDays WeekDay { get; set; }
        public int? ShiftId { get; set; }
    }
}
