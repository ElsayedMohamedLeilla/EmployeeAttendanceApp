using Dawem.Translations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dawem.Domain.Entities.Summons
{
    [Table(nameof(SummonMissingLogSanction) + LeillaKeys.S)]
    public class SummonMissingLogSanction : BaseEntity
    {
        #region Forign Key
        public int SummonMissingLogId { get; set; }
        [ForeignKey(nameof(SummonMissingLogId))]
        public SummonMissingLog SummonMissingLog { get; set; }
        public int SummonSanctionId { get; set; }
        [ForeignKey(nameof(SummonSanctionId))]
        public SummonSanction SummonSanction { get; set; }
        #endregion
        public int Code { get; set; }
        public bool Done { get; set; }
    }
}
