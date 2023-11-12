namespace Dawem.Models.Response.Attendances.Schedules
{
    public class GetSchedulesResponse
    {
        public List<GetSchedulesResponseModel> Schedules { get; set; }
        public int TotalCount { get; set; }
    }
}
