using Dawem.Models.Dtos.Dawem.Employees.Employees.GroupEmployees;
using Dawem.Models.Dtos.Dawem.Employees.Employees.GroupManagarDelegators;
using System.Text.Json.Serialization;

namespace Dawem.Models.Dtos.Dawem.Core.Groups
{
    public class UpdateGroupDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public List<int> EmployeeIds { get; set; }
        [JsonIgnore]
        public List<GroupEmployeeUpdateModelDTO> Employees { get; set; }

        public int? ManagerId { get; set; }

        public List<int> ManagerDelegatorIds { get; set; }

        [JsonIgnore]
        public List<GroupManagarDelegatorUpdateModelDTO> ManagerDelegators { get; set; }

        public List<int> ZoneIds { get; set; }
        [JsonIgnore]
        public List<ZoneGroupUpdateModelDTO> Zones { get; set; }
        public void MapGroupEmployees()
        {
            Employees = EmployeeIds
                .Select(employeeId => new GroupEmployeeUpdateModelDTO { EmployeeId = employeeId })
                .ToList();
        }
        public void MapGroupManagarDelegators()
        {
            ManagerDelegators = ManagerDelegatorIds
                .Select(employeeId => new GroupManagarDelegatorUpdateModelDTO { EmployeeId = employeeId })
                .ToList();
        }
        public void MapGroupZones()
        {
            Zones = ZoneIds != null ? ZoneIds
                .Select(zoneId => new ZoneGroupUpdateModelDTO { ZoneId = zoneId })
                .ToList() : null;
        }
    }
}
