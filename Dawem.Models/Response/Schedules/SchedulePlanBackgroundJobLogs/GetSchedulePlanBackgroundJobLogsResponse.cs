namespace Dawem.Models.Response.Schedules.SchedulePlanBackgroundJobLogs
{
    public class GetSchedulePlanBackgroundJobLogsResponse
    {
        public List<GetSchedulePlanBackgroundJobLogsResponseModel> SchedulePlanBackgroundJobLogs { get; set; }
        public int TotalCount { get; set; }
    }
}
