using SmartBusinessERP.Enums;
using SmartBusinessERP.Models.Request;

namespace SmartBusinessERP.Models.Criteria.Core
{
    public class GetStoresCriteria : BaseCriteria
    {
        public string? Name { get; set; }

        public StoreType? StoreType { get; set; }
     
    }
}
