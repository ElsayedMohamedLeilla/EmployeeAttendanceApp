namespace Dawem.Models.Response.AdminPanel.Subscriptions.Plans
{
    public class GetSubscriptionPaymentByIdResponseModel
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public int SubscriptionId { get; set; }
        public bool IsActive { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Notes { get; set; }
    }
}
