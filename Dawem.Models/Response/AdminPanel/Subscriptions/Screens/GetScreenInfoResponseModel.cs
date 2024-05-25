using Dawem.Models.Dtos.Dawem.Shared;

namespace Dawem.Models.Response.AdminPanel.Subscriptions.Screens
{
    public class GetScreenInfoResponseModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public string Notes { get; set; }
        public string ScreenIcon { get; set; }
        public string ScreenURL { get; set; }
        public List<string> Actions { get; set; }
        public List<NameTranslationGetInfoModel> NameTranslations { get; set; }
    }
}
