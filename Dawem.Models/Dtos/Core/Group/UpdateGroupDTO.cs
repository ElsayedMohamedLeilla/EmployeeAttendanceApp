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
        public void MapGroupEmployees()
        {
            GroupEmployees = EmployeeIdes
                .Select(employeeId => new GroupEmployeeUpdateModelDTO { EmployeeId = employeeId })
                .ToList();
        }
    }
}
