using Microsoft.AspNetCore.Http;

namespace Dawem.Models.Dtos.Employees.Employees
{
    public class CreateCompanyWithLogoModel
    {
        public string CreateCompanyModelString { get; set; }
        public IFormFile LogoImageFile { get; set; }
        public List<IFormFile> Attachments { get; set; }
    }
}
