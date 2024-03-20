using Dawem.Enums.Generals;
using Dawem.Translations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dawem.Domain.Entities.Subscriptions
{
    [Table(nameof(SubscriptionLog) + LeillaKeys.S)]
    public class SubscriptionLog : BaseEntity
    {
        public int Code { get; set; }
        public int SubscriptionId { get; set; }
        [ForeignKey(nameof(SubscriptionId))]
        public Subscription Subscription { get; set; }
        public SubscriptionLogType LogType { get; set; }
        public string LogTypeName { get; set; }
        public DateTime EndDate { get; set; }
    }

}
