using Dawem.Enums.General;
using SmartBusinessERP.Models.Request;

namespace SmartBusinessERP.Models.Criteria.Others
{
    public class CheckUserPermissionModel : BaseCriteria
    {
        public int UserId { get; set; }
        public ApplicationScreenType? Screen { get; set; }
        public ApiMethod? Method { get; set; }
    }
}
