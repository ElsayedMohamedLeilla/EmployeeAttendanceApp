using Dawem.Models.Dtos.Dawem.Shared;

namespace Dawem.Models.DTOs.Dawem.Screens.Screens
{
    public class BaseScreenModel : BaseCreateAndUpdateNameTranslation
    {
        public string Notes { get; set; }
        public bool IsActive { get; set; }
        public int Order { get; set; }
        public string Icon { get; set; }
        public string URL { get; set; }
    }
}
