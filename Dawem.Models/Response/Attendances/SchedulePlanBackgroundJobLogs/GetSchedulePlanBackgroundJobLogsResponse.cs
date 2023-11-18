namespace Dawem.Models.Response.Attendances.Schedules
{
    public class GetSchedulePlanBackgroundJobLogsResponse
    {
        public List<GetSchedulePlanBackgroundJobLogsResponseModel> SchedulePlanBackgroundJobLogs { get; set; }
        public int TotalCount { get; set; }
    }
}
