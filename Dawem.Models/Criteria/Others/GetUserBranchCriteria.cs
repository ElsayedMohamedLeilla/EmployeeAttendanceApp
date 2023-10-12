using SmartBusinessERP.Models.Request;

namespace SmartBusinessERP.Models.Criteria.Others
{
    public class GetUserBranchCriteria : BaseCriteria
    {
        public string? UserName { get; set; }
        public string? Password { get; set; }

    }
}
