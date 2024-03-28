using Microsoft.AspNetCore.Http;

namespace Dawem.Models.Requests.Assignments
{
    public class UpdateRequestAssignmentWithImageModel
    {
        public string UpdateRequestAssignmentModelString { get; set; }
        public List<IFormFile> Attachments { get; set; }
    }
}
