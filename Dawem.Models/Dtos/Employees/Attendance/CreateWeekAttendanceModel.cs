namespace Dawem.Models.Dtos.Employees.Employees
{
    public class CreateWeekAttendanceModel
    {
        public string Name { get; set; }
        public string Notes { get; set; }
        public bool IsActive { get; set; }
        public List<WeekAttendanceShiftCreateModel> WeekShifts { get; set; }

    }
}
