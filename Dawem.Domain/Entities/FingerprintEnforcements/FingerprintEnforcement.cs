using Dawem.Domain.Entities.Providers;
using Dawem.Enums.Generals;
using Dawem.Translations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dawem.Domain.Entities.Employees
{
    [Table(nameof(FingerprintEnforcement) + LeillaKeys.S)]
    public class FingerprintEnforcement : BaseEntity
    {
        #region Forign Key
        public int CompanyId { get; set; }
        [ForeignKey(nameof(CompanyId))]
        public Company Company { get; set; }
        #endregion
        public int Code { get; set; }
        public ForType ForType { get; set; }
        public bool? ForAllEmployees { get; set; }
        public DateTime FingerprintDate { get; set; }
        public int AllowedTime { get; set; }
        public TimeType TimeType { get; set; }
        public List<FingerprintEnforcementNotifyWay> FingerprintEnforcementNotifyWays { get; set; }
        public List<FingerprintEnforcementEmployee> FingerprintEnforcementEmployees { get; set; }
        public List<FingerprintEnforcementGroup> FingerprintEnforcementGroups { get; set; }
        public List<FingerprintEnforcementDepartment> FingerprintEnforcementDepartments { get; set; }
        public List<FingerprintEnforcementAction> FingerprintEnforcementActions { get; set; }
    }
}
