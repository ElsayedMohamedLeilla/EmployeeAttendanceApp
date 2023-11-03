namespace Dawem.Models.Response.Employees.Attendances.WeeksAttendances
{
    public class GetWeekAttendancesResponse
    {
        public List<GetWeekAttendancesResponseModel> WeekAttendances { get; set; }
        public int TotalCount { get; set; }
    }
}
