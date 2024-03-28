using Dawem.Enums.Generals;

namespace Dawem.Models.Response.Dawem.Summons.Summons
{
    public class GetSummonsResponseModel
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public ForType ForType { get; set; }
        public string ForTypeName { get; set; }
        public DateTime DateAndTime { get; set; }
        public SummonStatus SummonStatus { get; set; }
        public string SummonStatusName { get; set; }
        public int NumberOfTargetedEmployees { get; set; }
        public bool IsActive { get; set; }
    }
}
