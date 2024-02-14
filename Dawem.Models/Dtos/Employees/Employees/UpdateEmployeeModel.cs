using Dawem.Enums.Generals;
using Microsoft.AspNetCore.Http;
using System.Text.Json.Serialization;

namespace Dawem.Models.Dtos.Employees.Employees
{
    public class UpdateEmployeeModel
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public int DepartmentId { get; set; }
        public int? JobTitleId { get; set; }
        public int? ScheduleId { get; set; }
        public int? DirectManagerId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public string Address { get; set; }
        public IFormFile ProfileImageFile { get; set; }
        public string ProfileImageName { get; set; }
        public DateTime JoiningDate { get; set; }    
        public AttendanceType AttendanceType { get; set; }
        public EmployeeType EmployeeType { get; set; }
        public int? AnnualVacationBalance { get; set; }

        public int EmployeeNumber { get; set; }
        public bool IsActive { get; set; }

        public List<int> ZoneIds { get; set; }
        [JsonIgnore]
        public List<EmployeeZonesUpdateModelDTO> Zones { get; set; }

        public void MapEmployeeZones()
        {
            Zones = ZoneIds != null ? ZoneIds
                .Select(zoneId => new EmployeeZonesUpdateModelDTO { ZoneId = zoneId })
                .ToList() : null;
        }
    }
}
