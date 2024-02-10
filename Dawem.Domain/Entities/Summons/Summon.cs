using Dawem.Domain.Entities.Providers;
using Dawem.Enums.Generals;
using Dawem.Translations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dawem.Domain.Entities.Summons
{
    [Table(nameof(Summon) + LeillaKeys.S)]
    public class Summon : BaseEntity
    {
        #region Forign Key
        public int CompanyId { get; set; }
        [ForeignKey(nameof(CompanyId))]
        public Company Company { get; set; }
        #endregion
        public int Code { get; set; }
        public ForType ForType { get; set; }
        public bool? ForAllEmployees { get; set; }
        public DateTime DateAndTime { get; set; }
        public int AllowedTime { get; set; }
        public TimeType TimeType { get; set; }
        public List<SummonNotifyWay> SummonNotifyWays { get; set; }
        public List<SummonEmployee> SummonEmployees { get; set; }
        public List<SummonGroup> SummonGroups { get; set; }
        public List<SummonDepartment> SummonDepartments { get; set; }
        public List<SummonSanction> SummonSanctions { get; set; }
        public List<SummonMissingLog> SummonMissingLogs { get; set; }
    }
}
