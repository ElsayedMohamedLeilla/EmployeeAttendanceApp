using Dawem.Enums.Configration;

namespace Dawem.Models.Criteria.Others
{
    public class CheckUserPermissionModel : BaseCriteria
    {
        public int UserId { get; set; }
        public ApplicationScreenCode Screen { get; set; }
        public ApplicationAction Action { get; set; }
    }
}
