using Dawem.Models.Dtos.Dawem.Shared;

namespace Dawem.Models.Response.AdminPanel.Subscriptions.Screens
{
    public class GetScreenByIdResponseModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public string Notes { get; set; }
        public string Icon { get; set; }
        public string URL { get; set; }
        public List<NameTranslationModel> NameTranslations { get; set; }

    }
}
