namespace Dawem.Models.Response.Attendances.Schedules
{
    public class GetSchedulePlansForDropDownResponse
    {
        public List<GetSchedulesForDropDownResponseModel> Schedules { get; set; }
        public int TotalCount { get; set; }
    }
}
