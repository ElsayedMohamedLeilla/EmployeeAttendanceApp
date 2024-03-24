using Dawem.Enums.Generals;

namespace Dawem.Models.Criteria.Subscriptions
{
    public class CheckCompanySubscriptionModel
    {
        public int CompanyId { get; set; }
        public CheckCompanySubscriptionFromType FromType { get; set; }
    }
}
