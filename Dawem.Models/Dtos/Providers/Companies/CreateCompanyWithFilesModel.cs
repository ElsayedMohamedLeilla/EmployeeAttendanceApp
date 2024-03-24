using Microsoft.AspNetCore.Http;

namespace Dawem.Models.Dtos.Providers.Companies
{
    public class CreateCompanyWithFilesModel
    {
        public string CreateCompanyModelString { get; set; }
        public IFormFile LogoImageFile { get; set; }
        public List<IFormFile> Attachments { get; set; }
    }
}
