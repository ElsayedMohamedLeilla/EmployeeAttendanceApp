namespace Dawem.Models.Response.Dawem.Attendances
{
    public class GetCurrentEmployeeSchedulesResponse
    {
        public List<GetEmployeeScheduleResponseModel> Schedules { get; set; }
        public string TotalWorkingHours { get; set; }
    }
}
