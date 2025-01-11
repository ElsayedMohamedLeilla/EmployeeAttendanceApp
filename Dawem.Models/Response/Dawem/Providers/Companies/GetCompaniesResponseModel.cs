namespace Dawem.Models.Response.Dawem.Providers.Companies
{
    public class GetCompaniesResponseModel
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public string Name { get; set; }
        public string ResponsibleMobileNumber { get; set; }
        //public string CountryName { get; set; }
        public string LogoImagePath { get; set; }
        public string SubscriptionTypeName { get; set; }
       // public int NumberOfEmployees { get; set; }
        public bool IsActive { get; set; }
    }
}
