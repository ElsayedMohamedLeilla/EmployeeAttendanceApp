using Dawem.Enums.Generals;
using Dawem.Translations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dawem.Domain.Entities.Core.DefaultLookus
{
    [Table(nameof(DefaultLookup) + LeillaKeys.S)]
    public class DefaultLookup : BaseEntity
    {
        public LookupsType LookupType { get; set; }
        public int Code { get; set; }
        public string Name { get; set; }
        //public DefaultVacationType DefaultType { get; set; }
        public List<DefaultLookupsNameTranslation> NameTranslations { get; set; }


    }
}
