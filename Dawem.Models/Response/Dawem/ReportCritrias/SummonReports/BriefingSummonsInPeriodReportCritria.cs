using Dawem.Enums.Generals;

namespace Dawem.Models.Response.Dawem.ReportCritrias.SummonReports
{
    public class BriefingSummonsInPeriodReportCritria : BaseReportCritria
    {
        public List<int> ZoneIDs { get; set; }
        public List<int> DepartmentIDs { get; set; }
        public List<int> JobTitleIDs { get; set; }

    }
}
