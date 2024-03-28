namespace Dawem.Models.Response.Summons.Sanctions
{
    public class GetSanctionsResponse
    {
        public List<GetSanctionsResponseModel> Sanctions { get; set; }
        public int TotalCount { get; set; }
    }
}
