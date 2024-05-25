using Dawem.Domain.Entities.Subscriptions;
using Dawem.Enums.Generals;
using Dawem.Translations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dawem.Domain.Entities.Others
{
    [Table(nameof(Screen) + LeillaKeys.S)]
    public class Screen : BaseEntity
    {
        public int ScreenCode { get; set; }
        public string ScreenCodeName { get; set; }
        public AuthenticationType Type { get; set; }
        public string TypeName { get; set; }
        public List<ScreenNameTranslation> ScreenNameTranslations { get; set; }
        public List<ScreenAction> ScreenActions { get; set; }
    }
}
