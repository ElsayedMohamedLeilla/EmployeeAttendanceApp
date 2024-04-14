using Dawem.Translations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dawem.Domain.Entities.Subscriptions
{
    [Table(nameof(SubscriptionPayment) + LeillaKeys.S)]
    public class SubscriptionPayment : BaseEntity
    {
        public int Code { get; set; }
        public int SubscriptionId { get; set; }
        [ForeignKey(nameof(SubscriptionId))]
        public Subscription Subscription { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
    }

}
