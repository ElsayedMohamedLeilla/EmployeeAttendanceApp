namespace Dawem.Models.Dtos.Reports.AttendanceSummaryReport
{
    public class AttendanceSummaryForGridDTO
    {
        public List<AttendanceSummaryModelDTO> AttendanceSummaryData { get; set; }
        public int TotalCount { get; set; }
    }
}
