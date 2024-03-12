using Dawem.Enums.Generals;

namespace Dawem.Models.Response.Employees.Employees
{
    public class GetEmployeeByIdResponseModel
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public int EmployeeNumber { get; set; }
        public int? DepartmentId { get; set; }
        public int? DirectManagerId { get; set; }
        public int? JobTitleId { get; set; }
        public int? ScheduleId { get; set; }
        public string Name { get; set; }
        public string ProfileImageName { get; set; }
        public string ProfileImagePath { get; set; }
        public DateTime JoiningDate { get; set; }
        public AttendanceType AttendanceType { get; set; }
        public EmployeeType EmployeeType { get; set; }
        public int? AnnualVacationBalance { get; set; }
        public bool IsActive { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public string Address { get; set; }
        public string DisableReason { get; set; }
        public List<int> ZoneIds { get; set; }
        public bool AllowChangeFingerprintDeviceCodeForOneTime { get; set; }
    }
}
