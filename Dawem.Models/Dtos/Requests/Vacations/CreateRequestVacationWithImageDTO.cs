using Microsoft.AspNetCore.Http;

namespace Dawem.Models.Dtos.Employees.Employees
{
    public class CreateRequestVacationWithImageDTO
    {
        public string CreateRequestVacationModelString { get; set; }
        public List<IFormFile> Attachments { get; set; }
    }
}
