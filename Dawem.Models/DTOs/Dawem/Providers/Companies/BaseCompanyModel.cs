using Microsoft.AspNetCore.Http;
using System.Text.Json.Serialization;

namespace Dawem.Models.Dtos.Dawem.Providers.Companies
{
    public class BaseCompanyModel
    {
        public int CountryId { get; set; }
        public int? PreferredLanguageId { get; set; }
        public string WebSite { get; set; }
        public string HeadquarterAddress { get; set; }
        public double? HeadquarterLocationLatitude { get; set; }
        public double? HeadquarterLocationLongitude { get; set; }
        public string HeadquarterPostalCode { get; set; }
        public string Email { get; set; }
        public int? TotalNumberOfEmployees { get; set; }
        public bool ImportDefaultData { get; set; }
        public List<CompanyIndustryModel> Industries { get; set; }
        public List<CompanyBranchModel> Branches { get; set; }
        public IFormFile LogoImageFile { get; set; }
        public string LogoImageName { get; set; }
        public List<IFormFile> Attachments { get; set; }
        [JsonIgnore]
        public List<string> AttachmentsNames { get; set; }
    }
}