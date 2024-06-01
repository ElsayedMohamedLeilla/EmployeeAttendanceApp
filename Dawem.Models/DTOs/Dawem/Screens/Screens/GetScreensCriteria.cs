using Dawem.Enums.Permissions;
using Dawem.Models.Criteria;

namespace Dawem.Models.DTOs.Dawem.Screens.Screens
{
    public class GetScreensCriteria : BaseCriteria
    {
        public int? ScreenCode { get; set; }
        public int? ActionCode { get; set; }
        public int? PlanId { get; set; }
        public bool IsAllScreensAvailableInPlan { get; set; }
        public GroupOrScreenType? GroupOrScreenType { get; set; }
        public List<int> ScreensIds { get; set; }
    }
}
