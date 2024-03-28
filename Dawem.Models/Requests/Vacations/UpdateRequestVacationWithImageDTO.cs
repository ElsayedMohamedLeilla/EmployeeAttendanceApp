using Microsoft.AspNetCore.Http;

namespace Dawem.Models.Requests.Vacations
{
    public class UpdateRequestVacationWithImageDTO
    {
        public string UpdateRequestVacationModelString { get; set; }
        public List<IFormFile> Attachments { get; set; }
    }
}
