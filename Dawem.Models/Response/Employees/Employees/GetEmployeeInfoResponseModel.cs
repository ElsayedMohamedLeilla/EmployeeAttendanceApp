namespace Dawem.Models.Response.Employees.Employees
{
    public class GetEmployeeInfoResponseModel
    {
        public int Code { get; set; }
        public int EmployeeNumber { get; set; }
        public string DapartmentName { get; set; }
        public string DirectManagerName { get; set; }
        public string Name { get; set; }
        public string ProfileImagePath { get; set; }
        public string ProfileImageName { get; set; }
        public string JobTitleName { get; set; }
        public string ScheduleName { get; set; }
        public string AttendanceTypeName { get; set; }
        public string EmployeeTypeName { get; set; }
        public int? AnnualVacationBalance { get; set; }
        public DateTime JoiningDate { get; set; }
        public string Email { get; set; }
        public string MobileCountryFlagPath { get; set; }
        public string MobileCountryName { get; set; }
        public string MobileCountryCode { get; set; }
        public string MobileNumber { get; set; }
        public string Address { get; set; }
        public string DisableReason { get; set; }
        public bool IsActive { get; set; }
        public List<string> Zones { get; set; }
        public bool AllowChangeFingerprintMobileCode { get; set; }
    }
}
