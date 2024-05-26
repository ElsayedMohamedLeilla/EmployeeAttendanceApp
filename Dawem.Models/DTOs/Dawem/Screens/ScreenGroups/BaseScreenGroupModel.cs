using Dawem.Enums.Generals;
using Dawem.Models.Dtos.Dawem.Shared;

namespace Dawem.Models.DTOs.Dawem.Screens.ScreenGroups
{
    public class BaseScreenGroupModel : BaseCreateAndUpdateNameTranslation
    {
        public string Notes { get; set; }
        public bool IsActive { get; set; }
        public ScreenGroupType GroupType { get; set; }
        public int Order { get; set; }
        public string Icon { get; set; }
    }
}
