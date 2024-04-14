using Dawem.Enums.Generals;
using Dawem.Models.Criteria;

namespace Dawem.Models.Dtos.Dawem.Subscriptions
{
    public class GetSubscriptionsCriteria : BaseCriteria
    {
        public SubscriptionStatus? Status { get; set; }
        public SubscriptionType? Type { get; set; }
        public int? EndsAfterDaysFrom { get; set; }
        public int? EndsAfterDaysTo { get; set; }
        public DateTime? StartDateFrom { get; set; }
        public DateTime? StartDateTo { get; set; }
        public DateTime? EndDateFrom { get; set; }
        public DateTime? EndDateTo { get; set; }
        public int? DurationInDaysFrom { get; set; }
        public int? DurationInDaysTo { get; set; }
    }
}
