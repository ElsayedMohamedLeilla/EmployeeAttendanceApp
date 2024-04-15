namespace Dawem.Models.Response.AdminPanel.Subscriptions.Plans
{
    public class GetSubscriptionPaymentInfoResponseModel
    {
        public int Code { get; set; }
        public string SubscriptionInfo { get; set; }
        public bool IsActive { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Notes { get; set; }
    }
}
