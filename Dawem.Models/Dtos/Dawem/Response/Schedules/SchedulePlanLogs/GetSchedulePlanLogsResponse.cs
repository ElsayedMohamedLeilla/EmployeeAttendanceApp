namespace Dawem.Models.Response.Schedules.SchedulePlanLogs
{
    public class GetSchedulePlanLogsResponse
    {
        public List<GetSchedulePlanLogsResponseModel> SchedulePlanLogs { get; set; }
        public int TotalCount { get; set; }
    }
}
