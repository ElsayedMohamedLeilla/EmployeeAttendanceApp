using Dawem.Models.Criteria;

namespace Dawem.Models.Dtos.Reports.AttendanceSummaryReport
{
    public class AttendanceSummaryCritria : BaseCriteria
    {
        public List<int> EmployeesIds { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
    }
}
