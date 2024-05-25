using Dawem.Domain.Entities.Subscriptions;
using Dawem.Enums.Generals;
using Dawem.Translations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dawem.Domain.Entities.Others
{
    [Table(nameof(ScreenGroup) + LeillaKeys.S)]
    public class ScreenGroup : BaseEntity
    {
        public ScreenGroupType GroupType { get; set; }
        public string GroupTypeName { get; set; }
        public string GroupIcon { get; set; }
        public int Order { get; set; }
        public List<ScreenGroupNameTranslation> ScreenGroupNameTranslations { get; set; }
    }
}
