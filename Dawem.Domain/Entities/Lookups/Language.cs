using Dawem.Translations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dawem.Domain.Entities.Lookups
{
    [Table(nameof(Language) + LeillaKeys.S)]
    public class Language : BaseEntity
    {
        public string Name { get; set; }
        public string NativeName { get; set; }
        public string ISO2 { get; set; }
        public string ISO3 { get; set; }
    }
}
