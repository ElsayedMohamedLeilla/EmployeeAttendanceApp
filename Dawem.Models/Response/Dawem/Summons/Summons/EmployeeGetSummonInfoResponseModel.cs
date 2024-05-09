using Dawem.Enums.Generals;

namespace Dawem.Models.Response.Dawem.Summons.Summons
{
    public class EmployeeGetSummonInfoResponseModel
    {
        public int Code { get; set; }
        public DateTime LocalDateAndTime { get; set; }
        public string AllowedTimeName { get; set; }
        public List<SummonSancationModel> Sanctions { get; set; }
        public SummonStatus SummonStatus { get; set; }
        public string SummonStatusName { get; set; }
    }
}
