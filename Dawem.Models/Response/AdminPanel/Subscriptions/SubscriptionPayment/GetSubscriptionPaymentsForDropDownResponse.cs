namespace Dawem.Models.Response.AdminPanel.Subscriptions.SubscriptionPayment
{
    public class GetSubscriptionPaymentsForDropDownResponse
    {
        public List<GetSubscriptionPaymentsForDropDownResponseModel> SubscriptionPayments { get; set; }
        public int TotalCount { get; set; }
    }
}
