using Dawem.Enums.Permissions;

namespace Dawem.Models.Dtos.Dawem.Others
{
    public class ScreenWithAvailableActionsDTO
    {
        public ScreenWithAvailableActionsDTO()
        {
            AvailableActions = new List<DawemAdminApplicationAction>();
        }
        public int ScreenCode { get; set; }
        public string ScreenName { get; set; }
        public List<DawemAdminApplicationAction> AvailableActions { get; set; }
    }
}
