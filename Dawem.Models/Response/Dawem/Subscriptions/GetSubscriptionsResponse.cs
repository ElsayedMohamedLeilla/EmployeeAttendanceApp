namespace Dawem.Models.Response.Dawem.Subscriptions
{
    public class GetSubscriptionsResponse
    {
        public List<GetSubscriptionsResponseModel> Subscriptions { get; set; }
        public int TotalCount { get; set; }
    }
}
