using Dawem.Domain.Entities.Providers;
using Dawem.Translations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dawem.Domain.Entities.Summons
{
    [Table(nameof(SummonSanction) + LeillaKeys.S)]
    public class SummonSanction : BaseEntity
    {
        #region Forign Key
        public int CompanyId { get; set; }
        [ForeignKey(nameof(CompanyId))]
        public Company Company { get; set; }

        public int SummonId { get; set; }
        [ForeignKey(nameof(SummonId))]
        public Summon Summon { get; set; }

        public int SanctionId { get; set; }
        [ForeignKey(nameof(SanctionId))]
        public Sanction Sanction { get; set; }
        #endregion
    }
}
