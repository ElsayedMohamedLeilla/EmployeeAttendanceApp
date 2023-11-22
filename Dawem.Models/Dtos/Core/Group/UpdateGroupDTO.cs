using Dawem.Models.Dtos.Employees.Employees.GroupEmployees;
using Dawem.Models.Dtos.Employees.Employees.GroupManagarDelegators;
using System.Text.Json.Serialization;

namespace Dawem.Models.Dtos.Core.Group
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
    }
}
