namespace Dawem.Models.Response.AdminPanel.Subscriptions
{
    public class GetSubscriptionsResponse
    {
        public List<GetSubscriptionsResponseModel> Subscriptions { get; set; }
        public int TotalCount { get; set; }
    }
}
