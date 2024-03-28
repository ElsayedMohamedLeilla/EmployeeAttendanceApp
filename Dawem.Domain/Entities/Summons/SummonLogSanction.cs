using Dawem.Translations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dawem.Domain.Entities.Summons
{
    [Table(nameof(SummonLogSanction) + LeillaKeys.S)]
    public class SummonLogSanction : BaseEntity
    {
        #region Forign Key
        public int SummonLogId { get; set; }
        [ForeignKey(nameof(SummonLogId))]
        public SummonLog SummonLog { get; set; }
        public int SummonSanctionId { get; set; }
        [ForeignKey(nameof(SummonSanctionId))]
        public SummonSanction SummonSanction { get; set; }
        #endregion
        public bool Done { get; set; }
    }
}
