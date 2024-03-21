using Microsoft.AspNetCore.Http;

namespace Dawem.Models.Dtos.Employees.Employees
{
    public class UpdateCompanyWithLogoModel
    {
        public string UpdateCompanyModelString { get; set; }
        public IFormFile LogoImageFile { get; set; }
        public List<IFormFile> Attachments { get; set; }
    }
}
