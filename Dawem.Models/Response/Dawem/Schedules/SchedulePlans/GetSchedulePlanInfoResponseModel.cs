﻿using Dawem.Enums.Generals;

namespace Dawem.Models.Response.Dawem.Schedules.SchedulePlans
{
    public class GetSchedulePlanInfoResponseModel
    {
        public string ScheduleName { get; set; }
        public string EmployeeName { get; set; }
        public string GroupName { get; set; }
        public string DepartmentName { get; set; }
        public int Code { get; set; }
        public bool IsActive { get; set; }
        public string Notes { get; set; }
        public string SchedulePlanTypeName { get; set; }
        public ForType SchedulePlanType { get; set; }
        public DateTime DateFrom { get; set; }
    }
}
