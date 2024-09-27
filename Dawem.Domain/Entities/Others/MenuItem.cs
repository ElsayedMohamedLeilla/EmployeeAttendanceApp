using Dawem.Domain.Entities.Subscriptions;
using Dawem.Enums.Generals;
using Dawem.Enums.Permissions;
using Dawem.Translations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dawem.Domain.Entities.Others
{
    [Table(nameof(MenuItem) + LeillaKeys.S)]
    public class MenuItem : BaseEntity
    {
        public int? ParentId { get; set; }
        [ForeignKey(nameof(ParentId))]
        public MenuItem Parent { get; set; }
        public int? MenuItemCode { get; set; }
        public string MenuItemCodeName { get; set; }
        public GroupOrScreenType GroupOrScreenType { get; set; }
        public int Order { get; set; }
        public AuthenticationType AuthenticationType { get; set; }
        public string AuthenticationTypeName { get; set; }
        public string Icon { get; set; }
        public string URL { get; set; }
        public List<MenuItemNameTranslation> MenuItemNameTranslations { get; set; }
        public List<MenuItemAction> MenuItemActions { get; set; }
        public List<PlanScreen> PlanScreens { get; set; }
    }
}
