namespace Dawem.Models.Dtos.Employees.Employees
{
    public class UpdateWeekAttendanceModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Notes { get; set; }
        public bool IsActive { get; set; }
        public List<WeekAttendanceShiftUpdateModel> WeekShifts { get; set; }
    }
}
