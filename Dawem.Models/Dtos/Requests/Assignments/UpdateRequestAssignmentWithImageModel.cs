using Microsoft.AspNetCore.Http;

namespace Dawem.Models.Dtos.Requests.Tasks
{
    public class UpdateRequestAssignmentWithImageModel
    {
        public string UpdateRequestAssignmentModelString { get; set; }
        public List<IFormFile> Attachments { get; set; }
    }
}
