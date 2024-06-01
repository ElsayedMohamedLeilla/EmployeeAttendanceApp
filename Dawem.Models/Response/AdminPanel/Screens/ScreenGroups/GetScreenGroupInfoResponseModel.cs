using Dawem.Models.Dtos.Dawem.Shared;

namespace Dawem.Models.Response.AdminPanel.Subscriptions.Screens
{
    public class GetScreenGroupInfoResponseModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public string Notes { get; set; }
        public string Icon { get; set; }
        public string ParentName { get; set; }
        public string AuthenticationTypeName { get; set; }
        public int Order { get; set; }
        public List<string> Actions { get; set; }
        public List<NameTranslationGetInfoModel> NameTranslations { get; set; }
    }
}
