using Dawem.Enums.General;

namespace Dawem.Models.Criteria.Others
{
    public class GetActionLogsCriteria : BaseCriteria
    {
        public ApplicationScreenType? Screen { get; set; }
        public ApiMethod? Method { get; set; }
    }
}
