using Dawem.Models.Dtos.Dawem.Shared;

namespace Dawem.Models.Response.AdminPanel.Subscriptions.Screens
{
    public class GetScreenGroupByIdResponseModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public int? ParentId { get; set; }
        public int Order { get; set; }
        public string Notes { get; set; }
        public string Icon { get; set; }
        public List<NameTranslationModel> NameTranslations { get; set; }

    }
}
