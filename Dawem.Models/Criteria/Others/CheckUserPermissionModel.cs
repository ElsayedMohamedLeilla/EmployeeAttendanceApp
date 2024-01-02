using Dawem.Enums.Permissions;

namespace Dawem.Models.Criteria.Others
{
    public class CheckUserPermissionModel : BaseCriteria
    {
        public int UserId { get; set; }
        public ApplicationScreenCode ScreenCode { get; set; }
        public ApplicationAction ActionCode { get; set; }
    }
}
