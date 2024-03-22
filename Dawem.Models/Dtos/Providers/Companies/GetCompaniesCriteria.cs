using Dawem.Models.Criteria;

namespace Dawem.Models.Dtos.Providers.Companies
{
    public class GetCompaniesCriteria : BaseCriteria
    {
        public int? CountryId { get; set; }
    }
}
