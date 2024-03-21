namespace Dawem.Models.Response.Employees.Employees
{
    public class GetCompanyInfoResponseModel
    {
        public int Code { get; set; }
        public string CountryName { get; set; }
        public string PreferredLanguageName { get; set; }
        public string IdentityCode { get; set; }
        public string Name { get; set; }
        public string LogoImagePath { get; set; }
        public string WebSite { get; set; }
        public string HeadquarterAddress { get; set; }
        public string HeadquarterLocation { get; set; }
        public string HeadquarterPostalCode { get; set; }
        public string Email { get; set; }
        public int NumberOfEmployees { get; set; }
        public int TotalNumberOfEmployees { get; set; }
    }
}
