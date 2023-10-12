using SmartBusinessERP.Models.Request;

namespace SmartBusinessERP.Models.Criteria.UserManagement
{
    public class SmartUserSearchCriteria : BaseCriteria
    {
        public string? UserName { get; set; }
        public new int? Id { get; set; }
        public bool? IsActive { get; set; }
    }
}
