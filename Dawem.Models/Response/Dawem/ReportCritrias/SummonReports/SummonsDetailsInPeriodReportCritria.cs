using Dawem.Enums.Generals;

namespace Dawem.Models.Response.Dawem.ReportCritrias.SummonReports
{
    public class SummonsDetailsInPeriodReportCritria : BaseReportCritria
    {
        public List<int> EmployeeIDs { get; set; }
        public List<int> DepartmentIDs { get; set; }
        public List<int> JobTitleIDs { get; set; }

    }
}
