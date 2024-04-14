using Dawem.Enums.Generals;

namespace Dawem.Models.Criteria
{
    public class BaseCriteria
    {
        public int? CompanyId { get; set; }
        public int? BranchId { get; set; }
        public string Lang { get; set; }
        public int? Code { get; set; }
        public int? Id { get; set; }
        public List<int?> Ids { get; set; }
        public bool PagingEnabled { get; set; }
        public int? PageSize { get; set; }
        public int? PageNumber { get; set; }
        public string FreeText { get; set; }
        public OrderDirection? OrderByDirection { get; set; }
        public bool ForGridView { get; set; }
        public bool? IsActive { get; set; }
        public bool IsExport { get; set; }
        public AuthenticationType AuthenticationType { get; set; }
        public bool GetPagingEnabled()
        {
            return PagingEnabled && !IsExport;
        }
    }
}
