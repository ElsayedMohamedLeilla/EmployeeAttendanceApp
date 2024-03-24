namespace Dawem.Models.Response.Subscriptions.Plans
{
    public class GetPlansResponse
    {
        public List<GetPlansResponseModel> Plans { get; set; }
        public int TotalCount { get; set; }
    }
}
