namespace Dawem.Models.Response.Employees.Employees
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
    }
}
