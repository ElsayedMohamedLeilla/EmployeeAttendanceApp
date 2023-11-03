using Dawem.Enums.General;

namespace Dawem.Models.Dtos.Employees.Employees
{
    public class WeekAttendanceShiftModel
    {
        public WeekDays WeekDay { get; set; }
        public int? ShiftId { get; set; }
    }
}
