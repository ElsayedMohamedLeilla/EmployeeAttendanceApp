using Dawem.Enums.Generals;

namespace Dawem.Models.Response.Dawem.ReportCritrias
{
    public class SummonsDetailsGroupByEmployeeReportCritria : BaseReportCritria
    {
        public List<int> EmployeeIds { get; set; }
        public List<int> ZoneIds { get; set; }
        public List<int> DepartmentIds { get; set; }
        public List<int> JobTitleIds { get; set; }

    }
}
