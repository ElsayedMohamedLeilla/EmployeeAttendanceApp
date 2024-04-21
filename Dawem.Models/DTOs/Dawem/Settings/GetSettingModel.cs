using Dawem.Enums.Generals;

namespace Dawem.Models.Dtos.Dawem.Attendances
{
    public class GetSettingModel
    {
        public int Id { get; set; }
        public int SettingType { get; set; }
        public string SettingTypeName { get; set; }
        public SettingValueType ValueType { get; set; }
        public string ValueTypeName { get; set; }
        public dynamic Value { get; set; }
    }
}
