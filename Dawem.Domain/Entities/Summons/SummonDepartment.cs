using Dawem.Domain.Entities.Employees;
using Dawem.Domain.Entities.Providers;
using Dawem.Translations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dawem.Domain.Entities.Summons
{
    [Table(nameof(SummonDepartment) + LeillaKeys.S)]
    public class SummonDepartment : BaseEntity
    {
        #region Forign Key
        public int CompanyId { get; set; }
        [ForeignKey(nameof(CompanyId))]
        public Company Company { get; set; }

        public int SummonId { get; set; }
        [ForeignKey(nameof(SummonId))]
        public Summon Summon { get; set; }

        public int DepartmentId { get; set; }
        [ForeignKey(nameof(DepartmentId))]
        public Department Department { get; set; }
        #endregion
    }
}
