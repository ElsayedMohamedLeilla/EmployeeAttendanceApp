using Dawem.Enums.Generals;
using Dawem.Models.Criteria;

namespace Dawem.Models.Dtos.Dawem.Providers.Companies
{
    public class GetCompaniesCriteria : BaseCriteria
    {
        public SubscriptionType? SubscriptionType { get; set; }
        public int? CountryId { get; set; }
        public int? NumberOfEmployeesFrom { get; set; }
        public int? NumberOfEmployeesTo { get; set; }
    }
}
