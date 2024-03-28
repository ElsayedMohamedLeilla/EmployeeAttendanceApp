using Microsoft.AspNetCore.Http;

namespace Dawem.Models.Requests.Justifications
{
    public class UpdateRequestJustificationWithImageDTO
    {
        public string UpdateRequestJustificationModelString { get; set; }
        public List<IFormFile> Attachments { get; set; }
    }
}
