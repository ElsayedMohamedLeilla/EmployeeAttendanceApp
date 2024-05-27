using Dawem.Enums.Permissions;

namespace Dawem.Models.Dtos.Dawem.Others
{
    public class OldScreenWithAvailableActionsDTO
    {
        public OldScreenWithAvailableActionsDTO()
        {
            AvailableActions = new List<ApplicationActionCode>();
        }
        public int ScreenCode { get; set; }
        public string ScreenName { get; set; }
        public List<ApplicationActionCode> AvailableActions { get; set; }
    }
}
