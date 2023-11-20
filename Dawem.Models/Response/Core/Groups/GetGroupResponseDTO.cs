using Dawem.Models.Dtos.Employees.Employees;

namespace Dawem.Models.Response.Core.Groups
{
    public class GetGroupResponseDTO
    {
        public List<GroupEmployeeForGridDTO> Groups { get; set; }
        public int TotalCount { get; set; }
    }
}
