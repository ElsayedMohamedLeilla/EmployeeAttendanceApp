using Microsoft.AspNetCore.Http;

namespace Dawem.Models.Dtos.Providers.Companies
{
    public class UpdateCompanyWithFilesModel
    {
        public string UpdateCompanyModelString { get; set; }
        public IFormFile LogoImageFile { get; set; }
        public List<IFormFile> Attachments { get; set; }
    }
}
