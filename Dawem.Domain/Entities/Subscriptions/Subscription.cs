using Dawem.Domain.Entities.Providers;
using Dawem.Enums.Generals;
using Dawem.Translations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dawem.Domain.Entities.Subscriptions
{
    [Table(nameof(Subscription) + LeillaKeys.S)]
    public class Subscription : BaseEntity
    {
        public int Code { get; set; }
        public int PlanId { get; set; }
        [ForeignKey(nameof(PlanId))]
        public Plan Plan { get; set; }
        public int CompanyId { get; set; }
        [ForeignKey(nameof(CompanyId))]
        public Company Company { get; set; }
        public int DurationInDays { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public SubscriptionStatus Status { get; set; }
        public int RenewalCount { get; set; }
        public string FollowUpEmail { get; set; }
        public List<SubscriptionLog> SubscriptionLogs { get; set; }
    }
}