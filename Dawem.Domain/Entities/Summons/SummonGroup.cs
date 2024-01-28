using Dawem.Domain.Entities.Core;
using Dawem.Domain.Entities.Providers;
using Dawem.Translations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dawem.Domain.Entities.Summons
{
    [Table(nameof(SummonGroup) + LeillaKeys.S)]
    public class SummonGroup : BaseEntity
    {
        #region Forign Key
        public int CompanyId { get; set; }
        [ForeignKey(nameof(CompanyId))]
        public Company Company { get; set; }

        public int SummonId { get; set; }
        [ForeignKey(nameof(SummonId))]
        public Summon Summon { get; set; }

        public int GroupId { get; set; }
        [ForeignKey(nameof(GroupId))]
        public Group Group { get; set; }
        #endregion
    }
}
