﻿using Dawem.Enums.Generals;

namespace Dawem.Models.Response.Dawem.Schedules.SchedulePlanLogs
{
    public class GetSchedulePlanLogModel
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public int SchedulePlanId { get; set; }
        public int NewScheduleId { get; set; }
        public string ScheduleName { get; set; }
        public int? EmployeeId { get; set; }
        public int? GroupId { get; set; }
        public int? DepartmentId { get; set; }
        public ForType SchedulePlanType { get; set; }
        public DateTime DateFrom { get; set; }
    }
}
