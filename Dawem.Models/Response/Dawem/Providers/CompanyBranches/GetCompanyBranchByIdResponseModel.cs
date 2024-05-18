namespace Dawem.Models.Response.Dawem.Providers.Companies
{
    public class GetCompanyBranchByIdResponseModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
    }
}
