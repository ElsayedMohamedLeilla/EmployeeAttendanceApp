using Dawem.Domain.Entities.Others;
using Dawem.Translations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dawem.Domain.Entities.Subscriptions
{
    [Table(nameof(ScreenNameTranslation) + LeillaKeys.S)]
    public class ScreenNameTranslation : NameTranslation
    {
        public int ScreenId { get; set; }
        [ForeignKey(nameof(ScreenId))]
        public Screen Screen { get; set; }
    }

}
