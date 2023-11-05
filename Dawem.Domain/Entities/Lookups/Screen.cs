using Dawem.Translations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dawem.Domain.Entities.Lookups
{
    [Table(nameof(Screen) + LeillaKeys.S)]
    public class Screen : BaseEntity
    {
        public string Description { get; set; }
        public string DescriptionAr { get; set; }
        public string TitleAr { get; set; }
        public string TitleEn { get; set; }
        public string Icon { get; set; }
        public string LinkId { get; set; }
        public int? ParentId { get; set; }
        public Screen Parent { get; set; }
        public int Level { get; set; }
        public int Order { get; set; }
        public List<Screen> Children { get; set; }
        public new bool IsActive { get; set; }
    }
}
