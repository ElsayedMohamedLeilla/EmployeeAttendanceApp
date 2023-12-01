using Microsoft.AspNetCore.Http;

namespace Dawem.Models.Dtos.Employees.JobTitle
{
    public class UpdateRequestTaskModelDTO
    {
        public int Id { get; set; }
        public bool IsNecessary { get; set; }
        public bool ForEmployee { get; set; }
        public int? EmployeeId { get; set; }
        public int TaskTypeId { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public List<int> TaskEmployeeIds { get; set; }
        public bool IsActive { get; set; }
        public List<IFormFile> Attachments { get; set; }
    }
}
