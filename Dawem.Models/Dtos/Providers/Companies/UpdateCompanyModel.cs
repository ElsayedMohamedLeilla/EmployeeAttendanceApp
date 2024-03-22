using Microsoft.AspNetCore.Http;

namespace Dawem.Models.Dtos.Providers.Companies
{
    public class UpdateCompanyModel
    {
        public int Id { get; set; }
        public int? PreferredLanguageId { get; set; }
        public string WebSite { get; set; }
        public string HeadquarterAddress { get; set; }
        public string HeadquarterLocation { get; set; }
        public string HeadquarterPostalCode { get; set; }
        public string Email { get; set; }
        public int TotalNumberOfEmployees { get; set; }
        public bool ImportDefaultData { get; set; }
        public string LogoImageName { get; set; }
        public IFormFile LogoImageFile { get; set; }
        public List<CompanyIndustryModel> Industries { get; set; }
        public List<CompanyBranchModel> Branches { get; set; }
        public List<IFormFile> Attachments { get; set; }
        public List<string> AttachmentsNames { get; set; }
    }
}
