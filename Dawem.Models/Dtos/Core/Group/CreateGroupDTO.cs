using Dawem.Models.Dtos.Employees.Employees;

namespace Dawem.Models.Dtos.Core.Groups
{
    public class CreateGroupDTO
    {
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public List<GroupEmployeeCreateModelDTO> GroupEmployees { get; set; }

    }
}
