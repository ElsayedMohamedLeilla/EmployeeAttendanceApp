namespace Dawem.Models.Response.Dawem.Attendances
{
    public class GetEmployeeSchedulesResponse
    {
        public List<GetEmployeeScheduleResponseModel> Schedules { get; set; }
        public string TotalWorkingHours { get; set; }
    }
}
