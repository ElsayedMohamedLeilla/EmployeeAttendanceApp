﻿namespace Dawem.Models.Response.Dawem.Schedules.SchedulePlanLogs
{
    public class GetSchedulePlanLogsResponseModel
    {
        public int Id { get; set; }
        public string ScheduleName { get; set; }
        public string SchedulePlanTypeName { get; set; }
        public DateTime ApplyDate { get; set; }
        public int EmployeesNumberAppliedOn { get; set; }
    }
}
