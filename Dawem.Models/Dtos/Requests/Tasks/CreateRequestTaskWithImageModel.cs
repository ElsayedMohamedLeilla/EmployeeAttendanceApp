using Microsoft.AspNetCore.Http;

namespace Dawem.Models.Dtos.Requests.Tasks
{
    public class CreateRequestTaskWithImageModel
    {
        public string CreateRequestTaskModelString { get; set; }
        public List<IFormFile> Attachments { get; set; }
    }
}
