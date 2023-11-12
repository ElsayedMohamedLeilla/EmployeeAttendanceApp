namespace Dawem.Models.Response.Attendances.Schedules
{
    public class GetSchedulesForDropDownResponse
    {
        public List<GetSchedulesForDropDownResponseModel> Schedules { get; set; }
        public int TotalCount { get; set; }
    }
}
