namespace Dawem.Models.Response.AdminPanel.Subscriptions.Plans
{
    public class GetPlansResponse
    {
        public List<GetPlansResponseModel> Plans { get; set; }
        public int TotalCount { get; set; }
    }
}
