namespace Dawem.Models.Response.Dawem.Summons.Summons
{
    public class GetSummonsResponse
    {
        public List<GetSummonsResponseModel> Summons { get; set; }
        public int TotalCount { get; set; }
    }
}
