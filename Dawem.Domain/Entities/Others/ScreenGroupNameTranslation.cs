using Dawem.Domain.Entities.Others;
using Dawem.Translations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dawem.Domain.Entities.Subscriptions
{
    [Table(nameof(ScreenGroupNameTranslation) + LeillaKeys.S)]
    public class ScreenGroupNameTranslation : NameTranslation
    {
        public int ScreenGroupId { get; set; }
        [ForeignKey(nameof(ScreenGroupId))]
        public ScreenGroup ScreenGroup { get; set; }
    }

}
