using Dawem.Translations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dawem.Domain.Entities.Lookups
{
    [Table(nameof(OldNotUsedScreen) + LeillaKeys.S)]
    public class OldNotUsedScreen : BaseEntity
    {
        public string Description { get; set; }
        public string DescriptionAr { get; set; }
        public string TitleAr { get; set; }
        public string TitleEn { get; set; }
        public string Icon { get; set; }
        public string LinkId { get; set; }
        public int? ParentId { get; set; }
        public OldNotUsedScreen Parent { get; set; }
        public int Level { get; set; }
        public int Order { get; set; }
        public List<OldNotUsedScreen> Children { get; set; }
        public new bool IsActive { get; set; }
    }
}
