using Microsoft.AspNetCore.Http;

namespace Dawem.Models.Dtos.Requests.Tasks
{
    public class CreateRequestAssignmentWithImageModel
    {
        public string CreateRequestAssignmentModelString { get; set; }
        public List<IFormFile> Attachments { get; set; }
    }
}
