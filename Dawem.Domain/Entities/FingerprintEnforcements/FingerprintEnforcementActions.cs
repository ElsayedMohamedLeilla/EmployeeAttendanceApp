using Dawem.Domain.Entities.Providers;
using Dawem.Translations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dawem.Domain.Entities.Employees
{
    [Table(nameof(FingerprintEnforcementAction) + LeillaKeys.S)]
    public class FingerprintEnforcementAction : BaseEntity
    {
        #region Forign Key
        public int CompanyId { get; set; }
        [ForeignKey(nameof(CompanyId))]
        public Company Company { get; set; }

        public int FingerprintEnforcementId { get; set; }
        [ForeignKey(nameof(FingerprintEnforcementId))]
        public FingerprintEnforcement FingerprintEnforcement { get; set; }

        public int NonComplianceActionId { get; set; }
        [ForeignKey(nameof(NonComplianceActionId))]
        public NonComplianceAction NonComplianceAction { get; set; }
        #endregion
    }
}
