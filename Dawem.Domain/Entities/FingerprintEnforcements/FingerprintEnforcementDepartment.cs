using Dawem.Domain.Entities.Providers;
using Dawem.Translations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dawem.Domain.Entities.Employees
{
    [Table(nameof(FingerprintEnforcementDepartment) + LeillaKeys.S)]
    public class FingerprintEnforcementDepartment : BaseEntity
    {
        #region Forign Key
        public int CompanyId { get; set; }
        [ForeignKey(nameof(CompanyId))]
        public Company Company { get; set; }

        public int FingerprintEnforcementId { get; set; }
        [ForeignKey(nameof(FingerprintEnforcementId))]
        public FingerprintEnforcement FingerprintEnforcement { get; set; }

        public int DepartmentId { get; set; }
        [ForeignKey(nameof(DepartmentId))]
        public Department Department { get; set; }
        #endregion
    }
}
