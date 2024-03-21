namespace Dawem.Models.Response.Employees.Employees
{
    public class GetCompaniesResponseModel
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public string IdentityCode { get; set; }
        public string Name { get; set; }
        public string LogoImagePath { get; set; }
        public int NumberOfEmployees { get; set; }
        public int TotalNumberOfEmployees { get; set; }
    }
}
