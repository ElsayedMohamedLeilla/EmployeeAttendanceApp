using Dawem.Domain.Entities.Providers;
using Dawem.Enums.Generals;
using Dawem.Translations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dawem.Domain.Entities.Employees
{
    [Table(nameof(FingerprintEnforcementNotifyWay) + LeillaKeys.S)]
    public class FingerprintEnforcementNotifyWay : BaseEntity
    {
        #region Forign Key
        public int CompanyId { get; set; }
        [ForeignKey(nameof(CompanyId))]
        public Company Company { get; set; }

        public int FingerprintEnforcementId { get; set; }
        [ForeignKey(nameof(FingerprintEnforcementId))]
        public FingerprintEnforcement FingerprintEnforcement { get; set; }
        #endregion

        public NotifyWay NotifyWay { get; set; }
    }
}
