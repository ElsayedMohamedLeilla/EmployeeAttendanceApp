using Dawem.Enums.Generals;

namespace Dawem.Models.Response.Dawem.Summons.Summons
{
    public class EmployeeGetSummonsResponseModel
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public DateTime LocalDateAndTime { get; set; }
        public SummonStatus SummonStatus { get; set; }
        public string SummonStatusName { get; set; }
        public string AllowedTimeName { get; set; }
        public string EmployeeStatusName { get; set; }
    }
}
