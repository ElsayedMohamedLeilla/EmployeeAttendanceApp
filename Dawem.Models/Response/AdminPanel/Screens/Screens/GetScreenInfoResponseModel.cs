using Dawem.Models.Dtos.Dawem.Shared;

namespace Dawem.Models.Response.AdminPanel.Screens.Screens
{
    public class GetScreenInfoResponseModel
    {
        public string AuthenticationTypeName { get; set; }
        public string Name { get; set; }
        public string ParentName { get; set; }
        public bool IsActive { get; set; }
        public string Notes { get; set; }
        public string Icon { get; set; }
        public int Order { get; set; }
        public string URL { get; set; }
        public List<string> Actions { get; set; }
        public List<NameTranslationGetInfoModel> NameTranslations { get; set; }
    }
}
