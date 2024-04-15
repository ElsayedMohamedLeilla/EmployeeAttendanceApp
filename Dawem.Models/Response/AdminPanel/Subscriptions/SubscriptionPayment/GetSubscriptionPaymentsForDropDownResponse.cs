namespace Dawem.Models.Response.AdminPanel.Subscriptions.Plans
{
    public class GetSubscriptionPaymentsForDropDownResponse
    {
        public List<GetSubscriptionPaymentsForDropDownResponseModel> SubscriptionPayments { get; set; }
        public int TotalCount { get; set; }
    }
}
