using Dawem.Enums.General;

namespace Dawem.Models.Response.Employees.Employee
{
    public class GetEmployeeInfoResponseModel
    {
        public int Code { get; set; }
        public string DapartmentName { get; set; }
        public string Name { get; set; }
        public string ProfileImagePath { get; set; }
        public string JobTitleName { get; set; }
        public string AttendanceTypeName { get; set; }
        public int? AnnualVacationBalance { get; set; }
        public DateTime JoiningDate { get; set; }
        public bool IsActive { get; set; }
    }
}
