using Dawem.Enums.Permissions;

namespace Dawem.Models.Criteria.Others
{
    public class CheckUserPermissionModel : BaseCriteria
    {
        public int UserId { get; set; }
        public int ScreenCode { get; set; }
        public ApplicationAction ActionCode { get; set; }
        public string ActionName { get; set; }
    }
}
