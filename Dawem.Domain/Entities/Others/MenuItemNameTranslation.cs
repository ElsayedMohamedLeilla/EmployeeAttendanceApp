using Dawem.Translations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dawem.Domain.Entities.Others
{
    [Table(nameof(MenuItemNameTranslation) + LeillaKeys.S)]
    public class MenuItemNameTranslation : NameTranslation
    {
        public int MenuItemId { get; set; }
        [ForeignKey(nameof(MenuItemId))]
        public MenuItem MenuItem { get; set; }
    }

}
