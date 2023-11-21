using Dawem.Models.Dtos.Employees.Employees;
using System.Text.Json.Serialization;

namespace Dawem.Models.Dtos.Core.Groups
{
    public class UpdateGroupDTO
    {
        public int Id { get; set; }  
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public List<int> EmployeeIdes { get; set; }
        [JsonIgnore]
        public List<GroupEmployeeUpdateModelDTO> GroupEmployees { get; set; }

        public int GroupManagerId { get; set; }

        public List<int> GroupManagerDelegatorIdes { get; set; }

        [JsonIgnore]
        public List<GroupManagarDelegatorUpdateModelDTO> GroupManagerDelegators { get; set; }
        public void MapGroupEmployees()
        {
            GroupEmployees = EmployeeIdes
                .Select(employeeId => new GroupEmployeeUpdateModelDTO { EmployeeId = employeeId })
                .ToList();
        }
        public void MapGroupManagarDelegators()
        {
            GroupManagerDelegators = GroupManagerDelegatorIdes
                .Select(employeeId => new GroupManagarDelegatorUpdateModelDTO { EmployeeId = employeeId })
                .ToList();
        }
    }
}
