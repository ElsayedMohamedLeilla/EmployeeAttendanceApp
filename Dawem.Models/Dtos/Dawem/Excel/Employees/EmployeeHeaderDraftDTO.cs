namespace Dawem.Models.Dtos.Dawem.Excel.Employees
{
    public class EmployeeHeaderDraftDTO
    {
        public int EmployeeNumber { get; set; }
        public string EmployeeName { get; set; }
        public string DepartmentName { get; set; }
        public string JobTitle { get; set; }
        public string ScheduleName { get; set; }
        public string DirectManagerName { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public string Address { get; set; }
        public DateTime JoiningDate { get; set; }
        public string AttendanceType { get; set; }
        public string EmployeeType { get; set; }
        public bool IsActive { get; set; }
    }
}
