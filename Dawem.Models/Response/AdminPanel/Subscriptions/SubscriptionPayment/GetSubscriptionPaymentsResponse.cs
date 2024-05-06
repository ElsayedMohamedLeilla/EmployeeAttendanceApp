namespace Dawem.Models.Response.AdminPanel.Subscriptions.SubscriptionPayment
{
    public class GetSubscriptionPaymentsResponse
    {
        public List<GetSubscriptionPaymentsResponseModel> SubscriptionPayments { get; set; }
        public int TotalCount { get; set; }
    }
}
