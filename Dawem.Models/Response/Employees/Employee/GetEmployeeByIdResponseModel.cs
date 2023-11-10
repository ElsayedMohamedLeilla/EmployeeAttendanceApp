using Dawem.Enums.Generals;

namespace Dawem.Models.Response.Employees.Employee
{
    public class GetEmployeeByIdResponseModel
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public int DepartmentId { get; set; }
        public int? JobTitleId { get; set; }
        public string Name { get; set; }
        public string ProfileImageName { get; set; }
        public string ProfileImagePath { get; set; }
        public DateTime JoiningDate { get; set; }
        public AttendanceType AttendanceType { get; set; }
        public int? AnnualVacationBalance { get; set; }
        public bool IsActive { get; set; }
    }
}
