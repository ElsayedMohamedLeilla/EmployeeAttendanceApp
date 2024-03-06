using System.Text.Json.Serialization;

namespace Dawem.Models.Dtos.Employees.Departments
{
    public class CreateDepartmentModel
    {
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public string Notes { get; set; }
        public int? ParentId { get; set; }
        public int ManagerId { get; set; }
        public List<int> ManagerDelegatorIds { get; set; }

        [JsonIgnore]
        public List<DepartmentManagarDelegatorCreateModelDTO> ManagerDelegators { get; set; }

        public List<int> ZoneIds { get; set; }
        [JsonIgnore]
        public List<DepartmentZonesCreateModelDTO> Zones { get; set; }


        public void MapDepartmentManagarDelegators()
        {
            ManagerDelegators = ManagerDelegatorIds
                .Select(employeeId => new DepartmentManagarDelegatorCreateModelDTO { EmployeeId = employeeId })
                .ToList();
        }
        public void MapDepartmentZones()
        {
            Zones = ZoneIds != null ? ZoneIds
                .Select(zoneId => new DepartmentZonesCreateModelDTO { ZoneId = zoneId })
                .ToList(): null;
        }

    }
}
