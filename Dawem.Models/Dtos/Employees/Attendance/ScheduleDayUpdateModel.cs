using Dawem.Enums.General;

namespace Dawem.Models.Dtos.Employees.Employees
{
    public class ScheduleDayUpdateModel
    {
        public int Id { get; set; }
        public WeekDays WeekDay { get; set; }
        public int? ShiftId { get; set; }
    }
}
