using Microsoft.AspNetCore.Http;

namespace Dawem.Models.Dtos.Dawem.Providers.Companies
{
    public class CreateCompanyWithFilesModel
    {
        public string CreateCompanyModelString { get; set; }
        public IFormFile LogoImageFile { get; set; }
        public List<IFormFile> Attachments { get; set; }
    }
}
