using Dawem.Domain.Entities.Providers;
using Dawem.Enums.Generals;
using Dawem.Translations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dawem.Domain.Entities.Summons
{
    [Table(nameof(Sanction) + LeillaKeys.S)]
    public class Sanction : BaseEntity
    {
        #region Forign Key
        public int CompanyId { get; set; }
        [ForeignKey(nameof(CompanyId))]
        public Company Company { get; set; }
        #endregion
        public int Code { get; set; }
        public SanctionType Type { get; set; }
        public string Name { get; set; }
        public string WarningMessage { get; set; }
        public List<SummonSanction> SummonSanctions { get; set; }
    }
}
