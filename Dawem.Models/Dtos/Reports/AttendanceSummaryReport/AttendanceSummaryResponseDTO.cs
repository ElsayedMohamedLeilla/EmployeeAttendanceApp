namespace Dawem.Models.Dtos.Reports.AttendanceSummaryReport
{
    public class AttendanceSummaryResponseDTO
    {
        public List<AttendanceSummaryModel> AttendanceSmmaries { get; set; }
        public int TotalCount { get; set; }
    }
}
