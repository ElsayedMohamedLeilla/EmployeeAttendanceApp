using Microsoft.AspNetCore.Http;

namespace Dawem.Models.Requests.Tasks
{
    public class CreateRequestTaskWithImageModel
    {
        public string CreateRequestTaskModelString { get; set; }
        public List<IFormFile> Attachments { get; set; }
    }
}
