namespace Dawem.Models.Response.Dawem.Requests.Requests
{
    public class GetRequestsResponse
    {
        public List<GetRequestsResponseModel> Requests { get; set; }
        public int TotalCount { get; set; }
    }
}
