using Dawem.Domain.Entities.Employees;
using Dawem.Domain.Entities.Providers;
using Dawem.Domain.Entities.Schedules;
using Dawem.Domain.Entities.Subscriptions;
using Dawem.Enums.Generals;
using Dawem.Translations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dawem.Domain.Entities.Core.DefaultLookus
{
    [Table(nameof(DefaultLookup) + LeillaKeys.S)]
    public class DefaultLookup : BaseEntity
    {
        public LookupsType Type { get; set; }
        public int Code { get; set; }
        public string Name { get; set; }
        public List<DefaultLookupsTranslation> NameTranslations { get; set; }


    }
}
