namespace Dawem.Models.Response.Dawem.Schedules.Schedules
{
    public class GetSchedulesResponse
    {
        public List<GetSchedulesResponseModel> Schedules { get; set; }
        public int TotalCount { get; set; }
    }
}
