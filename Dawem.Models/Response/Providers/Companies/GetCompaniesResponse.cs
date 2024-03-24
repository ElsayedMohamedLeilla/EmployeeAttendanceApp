namespace Dawem.Models.Response.Providers.Companies
{
    public class GetCompaniesResponse
    {
        public List<GetCompaniesResponseModel> Companies { get; set; }
        public int TotalCount { get; set; }
    }
}
