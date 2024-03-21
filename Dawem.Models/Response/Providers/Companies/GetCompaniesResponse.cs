namespace Dawem.Models.Response.Employees.Employees
{
    public class GetCompaniesResponse
    {
        public List<GetCompaniesResponseModel> Companies { get; set; }
        public int TotalCount { get; set; }
    }
}
