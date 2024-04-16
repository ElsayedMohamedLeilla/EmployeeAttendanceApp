using Dawem.Enums.Generals;

namespace Dawem.Models.Response.AdminPanel.Subscriptions
{
    public class GetSubscriptionsResponseModel
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public string PlanName { get; set; }
        public string CompanyName { get; set; }
        public DateTime EndDate { get; set; }
        public SubscriptionStatus Status { get; set; }
        public string StatusName { get; set; }
        public bool IsWaitingForApproval { get; set; }
        public bool IsActive { get; set; }
    }
}
