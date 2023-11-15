using Dawem.Models.Dtos.Employees.Employees;

namespace Dawem.Models.Dtos.Core.Groups
{
    public class UpdateGroupDTO
    {
        public int Id { get; set; }  
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public List<GroupEmployeeUpdateModelDTO> GroupEmployees { get; set; }

    }
}
