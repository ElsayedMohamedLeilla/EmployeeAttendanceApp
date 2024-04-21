using Dawem.Domain.Entities.Providers;
using Dawem.Enums.Generals;
using Dawem.Translations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dawem.Domain.Entities.Dawem
{
    [Table(nameof(Setting) + LeillaKeys.S)]
    public class Setting : BaseEntity
    {
        public AuthenticationType Type { get; set; }
        public int? CompanyId { get; set; }
        [ForeignKey(nameof(CompanyId))]
        public Company Company { get; set; }
        public int SettingType { get; set; }
        public string TypeName { get; set; }
        public int GroupType { get; set; }
        public string GroupTypeName { get; set; }
        public SettingValueType ValueType { get; set; }
        public string ValueTypeName { get; set; }
        public string String { get; set; }
        public int? Integer { get; set; }
        public decimal? Decimal { get; set; }
        public bool? Bool { get; set; }
        public dynamic Value { get; set; }
    }
}
