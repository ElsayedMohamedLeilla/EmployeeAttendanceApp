using Dawem.Models.Dtos.Others;
using Dawem.Models.Dtos.Providers.Companies;

namespace Dawem.Models.Response.Providers.Companies
{
    public class GetCompanyByIdResponseModel
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public int CountryId { get; set; }
        public int? PreferredLanguageId { get; set; }
        public string IdentityCode { get; set; }
        public string Name { get; set; }
        public string LogoImageName { get; set; }
        public string LogoImagePath { get; set; }
        public string WebSite { get; set; }
        public string HeadquarterAddress { get; set; }
        public string HeadquarterLocation { get; set; }
        public string HeadquarterPostalCode { get; set; }
        public string Email { get; set; }
        public int NumberOfEmployees { get; set; }
        public int TotalNumberOfEmployees { get; set; }
        public bool IsActive { get; set; }
        public List<CompanyIndustryModel> Industries { get; set; }
        public List<CompanyBranchModel> Branches { get; set; }
        public List<FileDTO> Attachments { get; set; }
    }
}
