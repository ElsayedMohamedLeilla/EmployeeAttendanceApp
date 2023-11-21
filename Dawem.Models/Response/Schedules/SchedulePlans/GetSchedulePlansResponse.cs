namespace Dawem.Models.Response.Schedules.SchedulePlans
{
    public class GetSchedulePlansResponse
    {
        public List<GetSchedulePlansResponseModel> SchedulePlans { get; set; }
        public int TotalCount { get; set; }
    }
}
