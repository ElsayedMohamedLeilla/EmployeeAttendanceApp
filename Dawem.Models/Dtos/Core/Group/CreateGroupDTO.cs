using Dawem.Domain.Entities.Employees;
using Dawem.Models.Dtos.Employees.Employees.GroupEmployees;
using Dawem.Models.Dtos.Employees.Employees.GroupManagarDelegators;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text.Json.Serialization;

namespace Dawem.Models.Dtos.Core.Group
{
    public class CreateGroupDTO
    {
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public List<int> EmployeeIds { get; set; }
        [JsonIgnore] // This property will be excluded from JSON serialization
        public List<GroupEmployeeCreateModelDTO> Employees { get; set; }

        public int GroupManagerId { get; set; }

        public List<int> ManagerDelegatorIds { get; set; }

        [JsonIgnore]
        public List<GroupManagarDelegatorCreateModelDTO> ManagerDelegators { get; set; }

        public void MapGroupEmployees()
        {
            Employees = EmployeeIds
                .Select(employeeId => new GroupEmployeeCreateModelDTO { EmployeeId = employeeId })
                .ToList();
        }
        public void MapGroupManagarDelegators()
        {
            ManagerDelegators = ManagerDelegatorIds
                .Select(employeeId => new GroupManagarDelegatorCreateModelDTO { EmployeeId = employeeId })
                .ToList();
        }

    }
}
