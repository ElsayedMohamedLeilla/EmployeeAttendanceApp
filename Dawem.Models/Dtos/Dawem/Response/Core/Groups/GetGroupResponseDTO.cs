using Dawem.Models.Dtos.Dawem.Employees.Employees.GroupEmployees;

namespace Dawem.Models.Response.Core.Groups
{
    public class GetGroupResponseDTO
    {
        public List<GroupEmployeeForGridDTO> Groups { get; set; }
        public int TotalCount { get; set; }
    }
}
