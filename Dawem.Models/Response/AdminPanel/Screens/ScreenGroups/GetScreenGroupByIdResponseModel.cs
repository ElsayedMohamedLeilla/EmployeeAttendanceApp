using Dawem.Enums.Generals;
using Dawem.Models.Dtos.Dawem.Shared;

namespace Dawem.Models.Response.AdminPanel.Screens.ScreenGroups
{
    public class GetScreenGroupByIdResponseModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public int? ParentId { get; set; }
        public AuthenticationType AuthenticationType { get; set; }
        public int Order { get; set; }
        public string Notes { get; set; }
        public string Icon { get; set; }
        public List<NameTranslationModel> NameTranslations { get; set; }

    }
}
