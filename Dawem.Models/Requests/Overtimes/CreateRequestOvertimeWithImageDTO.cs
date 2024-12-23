using Microsoft.AspNetCore.Http;

namespace Dawem.Models.Requests.Justifications
{
    public class CreateRequestOvertimeWithImageDTO
    {
        public string CreateRequestOvertimeModelString { get; set; }
        public List<IFormFile> Attachments { get; set; }
    }
}
