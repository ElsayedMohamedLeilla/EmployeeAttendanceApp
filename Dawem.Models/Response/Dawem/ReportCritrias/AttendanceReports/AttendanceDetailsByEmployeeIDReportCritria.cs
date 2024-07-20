using Dawem.Enums.Generals;

namespace Dawem.Models.Response.Dawem.ReportCritrias.AttendanceReports
{
    public class AttendanceDetailsByEmployeeIDReportCritria : BaseReportCritria
    {
        public List<int> EmployeeIDs { get; set; }
        public List<int> ZoneIDs { get; set; }
        public List<int> DepartmentIDs { get; set; }
        public List<int> JobTitleIDs { get; set; }

    }
}
