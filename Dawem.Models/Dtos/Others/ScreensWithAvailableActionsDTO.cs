using Dawem.Enums.Generals;

namespace Dawem.Models.Dtos.Others
{
    public class ScreensWithAvailableActionsDTO
    {
        public ScreensWithAvailableActionsDTO()
        {
            AvailableActions = new List<ApiMethod>();
        }
        public ApplicationScreenType Screen { get; set; }
        public List<ApiMethod> AvailableActions { get; set; }
    }
}
