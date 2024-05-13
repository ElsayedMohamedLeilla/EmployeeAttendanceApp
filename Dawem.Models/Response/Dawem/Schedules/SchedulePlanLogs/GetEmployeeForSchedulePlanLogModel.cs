namespace Dawem.Models.Response.Dawem.Schedules.SchedulePlanLogs
{
    public class GetEmployeeForSchedulePlanLogModel
    {
        public int Id { get; set; }
        public int? ScheduleId { get; set; }
        public string ScheduleName { get; set; }
    }
}
