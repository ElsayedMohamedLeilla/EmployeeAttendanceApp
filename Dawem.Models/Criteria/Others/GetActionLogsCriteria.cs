using Dawem.Enums.General;
using SmartBusinessERP.Models.Request;

namespace SmartBusinessERP.Models.Criteria.Others
{
    public class GetActionLogsCriteria : BaseCriteria
    {
        public ApplicationScreenType? Screen { get; set; }
        public ApiMethod? Method { get; set; }
    }
}
