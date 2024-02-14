using Dawem.Models.Dtos.Others;

namespace Dawem.Models.Response.Requests.Justifications
{
    public class GetRequestJustificationByIdResponseDTO
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public bool IsNecessary { get; set; }
        public bool ForEmployee { get; set; }
        public int? EmployeeId { get; set; }
        public int JustificationTypeId { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public List<FileDTO> Attachments { get; set; }
        public bool IsActive { get; set; }
        public string Notes { get; set; }
    }
}
