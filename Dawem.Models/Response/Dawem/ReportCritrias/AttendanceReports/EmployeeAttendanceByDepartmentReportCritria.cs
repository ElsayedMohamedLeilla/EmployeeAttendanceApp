﻿namespace Dawem.Models.Response.Dawem.ReportCritrias.AttendanceReports
{
    public class EmployeeAttendanceByDepartmentReportCritria : BaseReportCritria
    {
        public List<int> EmployeeIds { get; set; }
        public List<int> DepartmentIds { get; set; }
        public List<int> JobTitleIds { get; set; }
        public List<int> ZoneIds { get; set; }
    }
}