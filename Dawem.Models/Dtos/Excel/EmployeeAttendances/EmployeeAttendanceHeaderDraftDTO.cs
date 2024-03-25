namespace Dawem.Models.Dtos.Excel.Employees
{
    public class EmployeeAttendanceHeaderDraftDTO
    {
        public string EmployeeName { get; set; }
        public DateTime LocalDate { get; set; }
        public string ShiftName { get; set; } // get shift check in , check out , allowed Minutes
        public TimeOnly Time { get; set; }


    }
}
