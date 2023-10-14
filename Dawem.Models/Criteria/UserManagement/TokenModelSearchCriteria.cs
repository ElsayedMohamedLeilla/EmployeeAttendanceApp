using Dawem.Enums.General;

namespace Dawem.Models.Criteria.UserManagement
{
    public class TokenModelSearchCriteria : BaseCriteria
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public bool RememberMe { get; set; }
        public int BranchId { get; set; }
        public IList<string> Roles { get; set; }
        public ApplicationType ApplicationType { get; set; }
    }
}
