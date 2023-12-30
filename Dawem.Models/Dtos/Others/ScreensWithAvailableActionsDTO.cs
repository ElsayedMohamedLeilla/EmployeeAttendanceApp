using Dawem.Enums.Configration;

namespace Dawem.Models.Dtos.Others
{
    public class ScreensWithAvailableActionsDTO
    {
        public ScreensWithAvailableActionsDTO()
        {
            AvailableActions = new List<Enums.Configration.ApplicationAction>();
        }
        public ApplicationScreenCode ScreenCode { get; set; }
        public string ScreenName { get; set; }
        public List<Enums.Configration.ApplicationAction> AvailableActions { get; set; }
    }
}
