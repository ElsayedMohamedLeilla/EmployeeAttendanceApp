using SmartBusinessERP.Enums;

namespace Dawem.Models.Criteria.Core
{
    public class GetStoresCriteria : BaseCriteria
    {
        public string? Name { get; set; }

        public StoreType? StoreType { get; set; }

    }
}
