using Dawem.Models.Dtos.Employees.Employees;

namespace Dawem.Models.Response.Core.Groups
{
    public class GetGroupInfoResponseDTO
    {
        public int Code { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public List<string> Employees { get; set; }
        public string Manager { get; set; }
        public List<string> ManagerDelegators { get; set; }
        public List<string> Zones { get; set; }




    }
}
