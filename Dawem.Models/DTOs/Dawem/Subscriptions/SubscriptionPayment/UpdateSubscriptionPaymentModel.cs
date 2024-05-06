namespace Dawem.Models.Dtos.Dawem.Subscriptions.SubscriptionPayment
{
    public class UpdateSubscriptionPaymentModel
    {
        public int Id { get; set; }
        public int SubscriptionId { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public bool IsActive { get; set; }
        public string Notes { get; set; }
    }
}
