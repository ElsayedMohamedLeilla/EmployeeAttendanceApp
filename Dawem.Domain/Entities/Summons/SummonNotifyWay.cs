using Dawem.Domain.Entities.Providers;
using Dawem.Enums.Generals;
using Dawem.Translations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dawem.Domain.Entities.Summons
{
    [Table(nameof(SummonNotifyWay) + LeillaKeys.S)]
    public class SummonNotifyWay : BaseEntity
    {
        #region Forign Key
        public int CompanyId { get; set; }
        [ForeignKey(nameof(CompanyId))]
        public Company Company { get; set; }

        public int SummonId { get; set; }
        [ForeignKey(nameof(SummonId))]
        public Summon Summon { get; set; }
        #endregion

        public NotifyWay NotifyWay { get; set; }
    }
}
