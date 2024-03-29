using Dawem.Enums.Generals;

namespace Dawem.Models.Response.Dawem.Summons.Summons
{
    public class GetSummonInfoResponseModel
    {
        public int Code { get; set; }
        public string ForTypeName { get; set; }
        public bool? ForAllEmployees { get; set; }
        public DateTime DateAndTime { get; set; }
        public string AllowedTimeName { get; set; }
        public List<string> NotifyWays { get; set; }
        public List<string> Employees { get; set; }
        public List<string> Groups { get; set; }
        public List<string> Departments { get; set; }
        public List<string> Sanctions { get; set; }
        public SummonStatus SummonStatus { get; set; }
        public string SummonStatusName { get; set; }
        public int NumberOfTargetedEmployees { get; set; }
        public bool IsActive { get; set; }
    }
}
