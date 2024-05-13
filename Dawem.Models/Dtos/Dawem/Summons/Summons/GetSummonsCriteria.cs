using Dawem.Enums.Generals;
using Dawem.Models.Criteria;

namespace Dawem.Models.Dtos.Dawem.Summons.Summons
{
    public class GetSummonsCriteria : BaseCriteria
    {
        public SummonStatus? Status { get; set; }
        public DateTime? Date { get; set; }
    }
}
