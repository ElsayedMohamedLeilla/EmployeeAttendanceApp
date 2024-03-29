using Dawem.Enums.Generals;

namespace Dawem.Models.Dtos.Dawem.Subscriptions
{
    public class CreateSubscriptionModel
    {
        public int PlanId { get; set; }
        public int CompanyId { get; set; }
        public int DurationInDays { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public SubscriptionStatus Status { get; set; }
        public int RenewalCount { get; set; }
        public string FollowUpEmail { get; set; }
        public string Notes { get; set; }
    }
}
