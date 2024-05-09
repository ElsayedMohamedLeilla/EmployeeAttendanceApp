using Dawem.Domain.Entities.Core;
using Dawem.Domain.Entities.Lookups;
using Dawem.Translations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dawem.Domain.Entities.Subscriptions
{
    [Table(nameof(NotificationTranslation) + LeillaKeys.S)]
    public class NotificationTranslation : BaseEntity
    {
        public int NotificationId { get; set; }
        [ForeignKey(nameof(NotificationId))]
        public Notification Notification { get; set; }
        public int LanguageId { get; set; }
        [ForeignKey(nameof(LanguageId))]
        public Language Language { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
    }

}
