namespace Dawem.Models.Response.Summons.Summons
{
    public class GetSummonsResponse
    {
        public List<GetSummonsResponseModel> Summons { get; set; }
        public int TotalCount { get; set; }
    }
}
