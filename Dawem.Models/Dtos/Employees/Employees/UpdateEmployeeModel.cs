using Dawem.Enums.Generals;
using Microsoft.AspNetCore.Http;

namespace Dawem.Models.Dtos.Employees.Employees
{
    public class UpdateEmployeeModel
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public int DepartmentId { get; set; }
        public string Name { get; set; }
        public string ProfileImageName { get; set; }
        public DateTime JoiningDate { get; set; }
        public IFormFile ProfileImageFile { get; set; }
        public int? JobTitleId { get; set; }
        public int? ScheduleId { get; set; }
        public AttendanceType AttendanceType { get; set; }
        public int? AnnualVacationBalance { get; set; }
        public bool IsActive { get; set; }
    }
}
