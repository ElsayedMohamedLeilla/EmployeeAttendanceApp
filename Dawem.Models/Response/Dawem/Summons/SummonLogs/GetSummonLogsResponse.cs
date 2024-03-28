namespace Dawem.Models.Response.Dawem.Summons.SummonLogs
{
    public class GetSummonLogsResponse
    {
        public List<GetSummonLogsResponseModel> SummonLogs { get; set; }
        public int TotalCount { get; set; }
    }
}
