namespace Dawem.Models.Response.Summons.Summons
{
    public class GetSummonMissingLogsResponse
    {
        public List<GetSummonMissingLogsResponseModel> SummonMissingLogs { get; set; }
        public int TotalCount { get; set; }
    }
}
