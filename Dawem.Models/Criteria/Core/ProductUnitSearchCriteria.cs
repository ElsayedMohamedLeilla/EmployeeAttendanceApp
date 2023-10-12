using SmartBusinessERP.Models.Request;

namespace SmartBusinessERP.Models.Criteria.Core
{
    public class ProductUnitSearchCriteria : BaseCriteria
    {
        public int? UnitId { get; set; }
        public int? ProductId { get; set; }
    }
}
