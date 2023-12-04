using Microsoft.AspNetCore.Http;

namespace Dawem.Models.Dtos.Requests.Justifications
{
    public class CreateRequestJustificationWithImageDTO
    {
        public string CreateRequestJustificationModelString { get; set; }
        public List<IFormFile> Attachments { get; set; }
    }
}
