using Dawem.Translations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dawem.Domain.Entities.Localization
{
    [Table(nameof(Translation) + LeillaKeys.S)]
    public class Translation : BaseEntity
    {
        [StringLength(500)]
        public string KeyWord { get; set; }
        public string TransWords { get; set; }
        public string Lang { get; set; }
    }
}
