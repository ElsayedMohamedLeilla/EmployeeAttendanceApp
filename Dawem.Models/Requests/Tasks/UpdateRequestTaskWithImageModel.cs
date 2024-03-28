using Microsoft.AspNetCore.Http;

namespace Dawem.Models.Requests.Tasks
{
    public class UpdateRequestTaskWithImageModel
    {
        public string UpdateRequestTaskModelString { get; set; }
        public List<IFormFile> Attachments { get; set; }
    }
}
