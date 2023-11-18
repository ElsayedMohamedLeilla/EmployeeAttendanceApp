using Dawem.Models.Dtos.Employees.Employees;

namespace Dawem.Models.Response.Core.Groups
{
    public class GetGroupInfoResponseDTO
    {
        public int Code { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public List<string> GroupEmployees { get; set; }

    }
}
