namespace Dawem.Models.Response.AdminPanel.Subscriptions.Plans
{
    public class GetSubscriptionPaymentsResponse
    {
        public List<GetSubscriptionPaymentsResponseModel> SubscriptionPayments { get; set; }
        public int TotalCount { get; set; }
    }
}
