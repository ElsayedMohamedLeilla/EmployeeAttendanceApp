using Dawem.Domain.Entities.Employees;
using Dawem.Domain.Entities.Providers;
using Dawem.Translations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dawem.Domain.Entities.Summons
{
    [Table(nameof(SummonLog) + LeillaKeys.S)]
    public class SummonLog : BaseEntity
    {
        #region Forign Key
        public int CompanyId { get; set; }
        [ForeignKey(nameof(CompanyId))]
        public Company Company { get; set; }
        public int EmployeeId { get; set; }
        [ForeignKey(nameof(EmployeeId))]
        public Employee Employee { get; set; }
        public int SummonId { get; set; }
        [ForeignKey(nameof(SummonId))]
        public Summon Summon { get; set; }
        #endregion
        public bool DoneSummon { get; set; }
        public bool DoneTakeActions { get; set; }
        public DateTime? DoneDate { get; set; }
        public List<SummonLogSanction> SummonLogSanctions { get; set; }
    }
}
