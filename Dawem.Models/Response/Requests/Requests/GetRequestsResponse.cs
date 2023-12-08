namespace Dawem.Models.Response.Requests.Requests
{
    public class GetRequestsResponse
    {
        public List<GetRequestsResponseModel> Requests { get; set; }
        public int TotalCount { get; set; }
    }
}
