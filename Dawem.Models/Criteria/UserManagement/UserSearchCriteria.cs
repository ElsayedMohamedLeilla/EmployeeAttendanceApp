namespace Dawem.Models.Criteria.UserManagement
{
    public class UserSearchCriteria : BaseCriteria
    {
        public string? UserName { get; set; }
        public new int? Id { get; set; }
        public bool? IsActive { get; set; }
    }
}
