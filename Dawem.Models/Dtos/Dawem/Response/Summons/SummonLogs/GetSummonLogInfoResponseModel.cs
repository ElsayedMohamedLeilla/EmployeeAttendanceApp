using Dawem.Enums.Generals;

namespace Dawem.Models.Response.Summons.SummonLogs
{
    public class GetSummonLogInfoResponseModel
    {
        public string EmployeeName { get; set; }
        public string SummonCode { get; set; }
        public DateTime SummonDate { get; set; }
        public string SummonForTypeName { get; set; }
        public string SummonAllowedTimeName { get; set; }
        public int SanctionsCount { get; set; }
        public bool DoneSummon { get; set; }
        public DateTime? DoneDate { get; set; }
        public SummonStatus SummonStatus { get; set; }
        public string SummonStatusName { get; set; }
        public List<string> SummonSanctions { get; set; }
    }
}
