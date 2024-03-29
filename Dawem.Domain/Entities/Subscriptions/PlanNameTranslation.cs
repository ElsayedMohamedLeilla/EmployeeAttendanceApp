using Dawem.Translations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dawem.Domain.Entities.Subscriptions
{
    [Table(nameof(PlanNameTranslation) + LeillaKeys.S)]
    public class PlanNameTranslation : NameTranslation
    {
        public int PlanId { get; set; }
        [ForeignKey(nameof(PlanId))]
        public Plan Plan { get; set; }
    }

}
