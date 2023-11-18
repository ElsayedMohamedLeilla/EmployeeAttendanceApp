namespace Dawem.Models.Response.Employees.Employee
{
    public class GetEmployeeInfoResponseModel
    {
        public int Code { get; set; }
        public string DapartmentName { get; set; }
        public string DirectManagerName { get; set; }
        public string Name { get; set; }
        public string ProfileImagePath { get; set; }
        public string JobTitleName { get; set; }
        public string ScheduleName { get; set; }
        public string AttendanceTypeName { get; set; }
        public int? AnnualVacationBalance { get; set; }
        public DateTime JoiningDate { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public string Address { get; set; }
        public string DisableReason { get; set; }
        public bool IsActive { get; set; }
    }
}
