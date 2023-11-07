namespace Dawem.Models.Response.Employees.Attendances.WeeksAttendances
{
    public class GetSchedulesResponse
    {
        public List<GetSchedulesResponseModel> Schedules { get; set; }
        public int TotalCount { get; set; }
    }
}
