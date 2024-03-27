namespace Dawem.Models.Response.Summons.Summons
{
    public class GetSummonLogsResponse
    {
        public List<GetSummonLogsResponseModel> SummonLogs { get; set; }
        public int TotalCount { get; set; }
    }
}
