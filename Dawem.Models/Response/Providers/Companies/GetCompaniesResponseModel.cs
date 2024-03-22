namespace Dawem.Models.Response.Providers.Companies
{
    public class GetCompaniesResponseModel
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public string IdentityCode { get; set; }
        public string Name { get; set; }
        public string LogoImagePath { get; set; }
        public int TotalNumberOfEmployees { get; set; }
        public bool IsActive { get; set; }
    }
}
