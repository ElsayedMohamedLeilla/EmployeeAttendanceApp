namespace Dawem.Models.Response.Summons.Summons
{
    public class GetSummonMissingLogsResponseModel
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public string EmployeeName { get; set; }
        public string SummonCode { get; set; }
        public DateTime SummonDate { get; set; }
        public int SanctionsCount { get; set; }
    }
}
