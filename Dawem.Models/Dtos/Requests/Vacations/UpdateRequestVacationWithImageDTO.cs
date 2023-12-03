using Microsoft.AspNetCore.Http;

namespace Dawem.Models.Dtos.Employees.Employees
{
    public class UpdateRequestVacationWithImageDTO
    {
        public string UpdateRequestVacationModelString { get; set; }
        public List<IFormFile> Attachments { get; set; }
    }
}
