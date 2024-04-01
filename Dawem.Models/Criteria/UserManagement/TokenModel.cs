using Dawem.Enums.Generals;

namespace Dawem.Models.Criteria.UserManagement
{
    public class TokenModel : BaseCriteria
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public bool RememberMe { get; set; }
        public int BranchId { get; set; }
        public IList<string> Responsibilities { get; set; }
        public ApplicationType ApplicationType { get; set; }
    }
}
