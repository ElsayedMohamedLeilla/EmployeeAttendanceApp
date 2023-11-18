using Dawem.Models.Dtos.Employees.Employees;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text.Json.Serialization;

namespace Dawem.Models.Dtos.Core.Groups
{
    public class CreateGroupDTO
    {
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public List<int> EmployeeIdes { get; set; }
        [JsonIgnore] // This property will be excluded from JSON serialization
        public List<GroupEmployeeCreateModelDTO> GroupEmployees { get; set; }

}
}
