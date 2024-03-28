namespace Dawem.Models.Response.Summons.SummonLogs
{
    public class GetSummonLogsResponse
    {
        public List<GetSummonLogsResponseModel> SummonLogs { get; set; }
        public int TotalCount { get; set; }
    }
}
