using Dawem.Enums.Generals;
using Dawem.Models.Dtos.Dawem.Shared;

namespace Dawem.Models.DTOs.Dawem.Screens.ScreenGroups
{
    public class BaseScreenGroupModel : BaseCreateAndUpdateNameTranslation
    {
        public int? ParentId { get; set; }
        public string Notes { get; set; }
        public bool IsActive { get; set; }
        public int Order { get; set; }
        public string Icon { get; set; }
        public AuthenticationType AuthenticationType { get; set; }
    }
}
