using Dawem.Enums.Generals;
using Dawem.Models.Criteria;

namespace Dawem.Models.Dtos.Dawem.Summons.Summons
{
    public class GetSummonLogsCriteria : BaseCriteria
    {
        public SummonDoneStatus? SummonDoneStatus { get; set; }
        public DateTime? SummonDate { get; set; }
        public int? EmployeeNumber {get; set;}
        public int? SummonCode { get; set; }
    }
}
