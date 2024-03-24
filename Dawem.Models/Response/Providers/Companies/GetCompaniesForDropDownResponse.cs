namespace Dawem.Models.Response.Providers.Companies
{
    public class GetCompaniesForDropDownResponse
    {
        public List<GetCompaniesForDropDownResponseModel> Companies { get; set; }
        public int TotalCount { get; set; }
    }
}
