namespace Dawem.Models.Response.Schedules.SchedulePlanBackgroundJobLogs
{
    public class GetSchedulePlanBackgroundJobLogsResponseModel
    {
        public int Id { get; set; }
        public string ScheduleName { get; set; }
        public string SchedulePlanTypeName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime FinishDate { get; set; }
        public int EmployeesNumberAppliedOn { get; set; }
    }
}
