using Dawem.Models.Dtos.Dawem.Settings;

namespace Dawem.Models.Response.AdminPanel.Settings
{
    public class GetSettingsResponse
    {
        public List<GetSettingGroupModel> SettingsGroups { get; set; }
    }
}
