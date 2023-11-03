namespace Dawem.Models.Response.Employees.Attendances.WeeksAttendances
{
    public class GetWeekAttendancesForDropDownResponse
    {
        public List<GetWeekAttendancesForDropDownResponseModel> WeekAttendances { get; set; }
        public int TotalCount { get; set; }
    }
}
