using Dawem.Models.Dtos.Dawem.Shared;

namespace Dawem.Models.DTOs.Dawem.Screens
{
    public class BaseScreenModel : BaseCreateAndUpdateNameTranslation
    {
        public string Notes { get; set; }
        public bool IsActive { get; set; }
        public int Order { get; set; }
        public string ScreenIcon { get; set; }
        public string ScreenURL { get; set; }
    }
}
