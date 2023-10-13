using Dawem.Models.Criteria;

namespace Dawem.Models.Criteria.Provider
{
    public class ValidateUserBranchSearchCriteria : BaseCriteria
    {
        public int UserId { get; set; }

        public string UserName { get; set; }

        public new int BranchId { get; set; }
    }
}
