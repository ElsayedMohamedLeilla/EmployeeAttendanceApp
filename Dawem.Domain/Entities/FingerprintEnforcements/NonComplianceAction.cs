using Dawem.Domain.Entities.Providers;
using Dawem.Translations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dawem.Domain.Entities.Employees
{
    [Table(nameof(NonComplianceAction) + LeillaKeys.S)]
    public class NonComplianceAction : BaseEntity
    {
        #region Forign Key
        public int CompanyId { get; set; }
        [ForeignKey(nameof(CompanyId))]
        public Company Company { get; set; }
        #endregion
        public int Code { get; set; }
        public string Name { get; set; }
    }
}
