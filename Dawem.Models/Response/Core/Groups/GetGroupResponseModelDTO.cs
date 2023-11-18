using Dawem.Models.Dtos.Employees.Employees;

namespace Dawem.Models.Response.Core.Groups
{
    public class GetGroupResponseModelDTO
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }

    }
}
