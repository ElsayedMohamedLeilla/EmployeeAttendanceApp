using Dawem.Enums.Generals;

namespace Dawem.Models.Response.Dawem.Summons.SummonLogs
{
    public class GetSummonLogsResponseModel
    {
        public int Id { get; set; }
        public string EmployeeName { get; set; }
        public string SummonCode { get; set; }
        public DateTime SummonDate { get; set; }
        public int SanctionsCount { get; set; }
        public bool DoneSummon { get; set; }
        public SummonStatus SummonStatus { get; set; }
        public string SummonStatusName { get; set; }
        public DateTime? DoneDate { get; set; }
    }
}
