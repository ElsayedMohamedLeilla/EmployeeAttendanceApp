using Microsoft.AspNetCore.Http;

namespace Dawem.Models.Requests.Vacations
{
    public class CreateRequestVacationWithImageDTO
    {
        public string CreateRequestVacationModelString { get; set; }
        public List<IFormFile> Attachments { get; set; }
    }
}
