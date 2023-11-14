namespace Dawem.Models.Response.Attendances.Schedules
{
    public class GetSchedulePlansResponse
    {
        public List<GetSchedulePlansResponseModel> SchedulePlans { get; set; }
        public int TotalCount { get; set; }
    }
}
