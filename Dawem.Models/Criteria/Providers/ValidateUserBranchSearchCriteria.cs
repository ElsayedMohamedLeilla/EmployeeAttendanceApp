namespace Dawem.Models.Criteria.Providers
{
    public class ValidateUserBranchSearchCriteria : BaseCriteria
    {
        public int UserId { get; set; }

        public string UserName { get; set; }

        public new int BranchId { get; set; }
    }
}
