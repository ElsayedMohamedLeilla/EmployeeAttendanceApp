using Dawem.Domain.Entities.Lookups;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dawem.Domain.Entities
{
    public class NameTranslation : BaseEntity
    {
        public int LanguageId { get; set; }
        [ForeignKey(nameof(LanguageId))]
        public Language Language { get; set; }
        public string Name { get; set; }
    }
}