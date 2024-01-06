namespace Dawem.Models.Response.Schedules.SchedulePlanLogs
{
    public class GetSchedulePlanLogEmployeesResponse
    {
        public List<GetSchedulePlanLogEmployeeInfoModel> Employees { get; set; }
        public int TotalCount { get; set; }
    }
}
