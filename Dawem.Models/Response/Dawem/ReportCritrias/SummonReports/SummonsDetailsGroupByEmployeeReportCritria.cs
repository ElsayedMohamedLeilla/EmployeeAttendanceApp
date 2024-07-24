using Dawem.Enums.Generals;

namespace Dawem.Models.Response.Dawem.ReportCritrias
{
    public class SummonsDetailsGroupByEmployeeReportCritria : BaseReportCritria
    {
        public List<int> EmployeeIDs { get; set; }
        public List<int> DepartmentIDs { get; set; }
        public List<int> JobTitleIDs { get; set; }

    }
}
