using Dawem.Domain.Entities.Others;
using Dawem.Enums.Permissions;
using Dawem.Translations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dawem.Domain.Entities.Subscriptions
{
    [Table(nameof(MenuItemAction) + LeillaKeys.S)]
    public class MenuItemAction : BaseEntity
    {
        public int MenuItemId { get; set; }
        [ForeignKey(nameof(MenuItemId))]
        public MenuItem MenuItem { get; set; }
        public ApplicationActionCode ActionCode { get; set; }
        public string ActionCodeName { get; set; }
    }

}
