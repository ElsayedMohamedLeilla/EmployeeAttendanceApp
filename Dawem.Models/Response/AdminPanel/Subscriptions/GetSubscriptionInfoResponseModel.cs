using Dawem.Enums.Generals;

namespace Dawem.Models.Response.AdminPanel.Subscriptions
{
    public class GetSubscriptionInfoResponseModel
    {
        public int Code { get; set; }
        public string PlanName { get; set; }
        public string CompanyName { get; set; }
        public int DurationInDays { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public SubscriptionStatus Status { get; set; }
        public string StatusName { get; set; }
        public int RenewalCount { get; set; }
        public string FollowUpEmail { get; set; }
        public bool IsWaitingForApproval { get; set; }
        public string Notes { get; set; }
    }
}
