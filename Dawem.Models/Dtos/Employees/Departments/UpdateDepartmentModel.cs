using System.Text.Json.Serialization;

namespace Dawem.Models.Dtos.Employees.Departments
{
    public class UpdateDepartmentModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? ParentId { get; set; }
        public bool IsActive { get; set; }
        public string Notes { get; set; }

        public int ManagerId { get; set; }

        public List<int> ManagerDelegatorIds { get; set; }

        [JsonIgnore]
        public List<DepartmentManagarDelegatorUpdateModelDTO> ManagerDelegators { get; set; }

        public List<int> ZoneIds { get; set; }
        [JsonIgnore]
        public List<DepartmentZonesUpdateModelDTO> Zones { get; set; }


        public void MapDepartmentManagarDelegators()
        {
            ManagerDelegators = ManagerDelegatorIds
                .Select(employeeId => new DepartmentManagarDelegatorUpdateModelDTO { EmployeeId = employeeId })
                .ToList();
        }
        public void MapDepartmentZones()
        {
            Zones = ZoneIds != null ? ZoneIds
                .Select(zoneId => new DepartmentZonesUpdateModelDTO { ZoneId = zoneId })
                .ToList() : null;
        }
    }
}
