namespace Dawem.Models.Dtos.Dawem.Settings
{
    public class GetSettingGroupModel
    {
        public int GroupType { get; set; }
        public string GroupTypeName { get; set; }
        public List<GetSettingModel> Settings { get; set; }
    }
}
