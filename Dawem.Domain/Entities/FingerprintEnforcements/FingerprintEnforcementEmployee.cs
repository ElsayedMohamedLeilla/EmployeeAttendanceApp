using Dawem.Domain.Entities.Providers;
using Dawem.Translations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dawem.Domain.Entities.Employees
{
    [Table(nameof(FingerprintEnforcementEmployee) + LeillaKeys.S)]
    public class FingerprintEnforcementEmployee : BaseEntity
    {
        #region Forign Key
        public int CompanyId { get; set; }
        [ForeignKey(nameof(CompanyId))]
        public Company Company { get; set; }

        public int FingerprintEnforcementId { get; set; }
        [ForeignKey(nameof(FingerprintEnforcementId))]
        public FingerprintEnforcement FingerprintEnforcement { get; set; }

        public int EmployeeId { get; set; }
        [ForeignKey(nameof(EmployeeId))]
        public Employee Employee { get; set; }

        #endregion
    }
}
