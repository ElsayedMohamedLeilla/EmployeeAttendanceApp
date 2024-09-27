namespace Dawem.Models.Response.AdminPanel.Subscriptions.SubscriptionPayment
{
    public class GetSubscriptionPaymentsResponseModel
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public string SubscriptionInfo { get; set; }
        public bool IsActive { get; set; }
        public decimal Amount { get; set; }
    }
}
