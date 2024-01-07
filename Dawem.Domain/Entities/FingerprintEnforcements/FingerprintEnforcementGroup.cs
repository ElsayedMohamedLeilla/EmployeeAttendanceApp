using Dawem.Domain.Entities.Core;
using Dawem.Domain.Entities.Providers;
using Dawem.Translations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dawem.Domain.Entities.Employees
{
    [Table(nameof(FingerprintEnforcementGroup) + LeillaKeys.S)]
    public class FingerprintEnforcementGroup : BaseEntity
    {
        #region Forign Key
        public int CompanyId { get; set; }
        [ForeignKey(nameof(CompanyId))]
        public Company Company { get; set; }

        public int FingerprintEnforcementId { get; set; }
        [ForeignKey(nameof(FingerprintEnforcementId))]
        public FingerprintEnforcement FingerprintEnforcement { get; set; }

        public int GroupId { get; set; }
        [ForeignKey(nameof(GroupId))]
        public Group Group { get; set; }
        #endregion
    }
}
