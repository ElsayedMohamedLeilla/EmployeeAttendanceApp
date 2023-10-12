using SmartBusinessERP.Models.Request;

namespace SmartBusinessERP.Models.Criteria.Provider
{
    public class ValidateUserBranchSearchCriteria : BaseCriteria
    {
        public int UserId { get; set; }

        public string UserName { get; set; }

        public new int BranchId { get; set; }
    }
}
