using Microsoft.AspNetCore.Http;

namespace Dawem.Models.Requests.Justifications
{
    public class UpdateRequestOvertimeWithImageDTO
    {
        public string UpdateRequestOvertimeModelString { get; set; }
        public List<IFormFile> Attachments { get; set; }
    }
}
