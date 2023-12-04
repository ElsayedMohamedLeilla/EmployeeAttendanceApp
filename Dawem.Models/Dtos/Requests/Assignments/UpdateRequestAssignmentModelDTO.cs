using Microsoft.AspNetCore.Http;

namespace Dawem.Models.Dtos.Requests.Tasks
{
    public class UpdateRequestAssignmentModelDTO
    {
        public int Id { get; set; }
        public bool IsNecessary { get; set; }
        public bool ForEmployee { get; set; }
        public int? EmployeeId { get; set; }
        public int AssignmentTypeId { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public List<IFormFile> Attachments { get; set; }
        public List<string> AttachmentsNames { get; set; }
        public string Notes { get; set; }
    }
}
