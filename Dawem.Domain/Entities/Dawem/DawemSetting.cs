using Dawem.Enums.Generals;
using Dawem.Translations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dawem.Domain.Entities.Providers
{
    [Table(nameof(DawemSetting) + LeillaKeys.S)]
    public class DawemSetting : BaseEntity
    {
        public DawemSettingType Type { get; set; }
        public string TypeName { get; set; }
        public DawemSettingGroupType GroupType { get; set; }
        public string GroupTypeName { get; set; }      
        public DawemSettingValueType ValueType { get; set; }
        public string ValueTypeName { get; set; }
        public string String { get; set; }
        public int? Integer { get; set; }
        public decimal? Decimal { get; set; }
    }
}
