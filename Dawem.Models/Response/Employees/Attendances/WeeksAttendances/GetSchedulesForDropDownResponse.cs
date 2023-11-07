namespace Dawem.Models.Response.Employees.Attendances.WeeksAttendances
{
    public class GetSchedulesForDropDownResponse
    {
        public List<GetSchedulesForDropDownResponseModel> Schedules { get; set; }
        public int TotalCount { get; set; }
    }
}
