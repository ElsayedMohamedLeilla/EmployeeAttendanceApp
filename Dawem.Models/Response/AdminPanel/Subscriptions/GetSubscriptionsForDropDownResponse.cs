namespace Dawem.Models.Response.AdminPanel.Subscriptions.Plans
{
    public class GetSubscriptionsForDropDownResponse
    {
        public List<GetSubscriptionsForDropDownResponseModel> Subscriptions { get; set; }
        public int TotalCount { get; set; }
    }
}
