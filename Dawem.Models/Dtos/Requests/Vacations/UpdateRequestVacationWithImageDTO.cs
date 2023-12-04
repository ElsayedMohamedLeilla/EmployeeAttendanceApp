using Microsoft.AspNetCore.Http;

namespace Dawem.Models.Dtos.Requests.Vacations
{
    public class UpdateRequestVacationWithImageDTO
    {
        public string UpdateRequestVacationModelString { get; set; }
        public List<IFormFile> Attachments { get; set; }
    }
}
