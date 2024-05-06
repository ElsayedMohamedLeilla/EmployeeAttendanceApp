namespace Dawem.Models.Response.AdminPanel.Subscriptions
{
    public class GetSubscriptionsForDropDownResponse
    {
        public List<GetSubscriptionsForDropDownResponseModel> Subscriptions { get; set; }
        public int TotalCount { get; set; }
    }
}
