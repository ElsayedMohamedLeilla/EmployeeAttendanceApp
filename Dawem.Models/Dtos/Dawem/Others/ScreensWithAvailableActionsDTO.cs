using Dawem.Enums.Permissions;

namespace Dawem.Models.Dtos.Dawem.Others
{
    public class ScreensWithAvailableActionsDTO
    {
        public ScreensWithAvailableActionsDTO()
        {
            AvailableActions = new List<ApplicationAction>();
        }
        public int ScreenCode { get; set; }
        public string ScreenName { get; set; }
        public List<ApplicationAction> AvailableActions { get; set; }
    }
}
