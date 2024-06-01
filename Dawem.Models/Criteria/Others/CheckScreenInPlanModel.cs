using Dawem.Enums.Generals;

namespace Dawem.Models.Criteria.Others
{
    public class CheckScreenInPlanModel : BaseCriteria
    {
        public int ScreenCode { get; set; }
        public ApplicationType ApplicationType { get; set; }
    }
}
