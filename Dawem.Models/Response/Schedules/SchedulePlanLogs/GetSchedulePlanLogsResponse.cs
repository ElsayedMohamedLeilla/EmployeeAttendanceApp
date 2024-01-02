namespace Dawem.Models.Response.Schedules.SchedulePlanBackgroundJobLogs
{
    public class GetSchedulePlanLogsResponse
    {
        public List<GetSchedulePlanLogsResponseModel> SchedulePlanLogs { get; set; }
        public int TotalCount { get; set; }
    }
}
