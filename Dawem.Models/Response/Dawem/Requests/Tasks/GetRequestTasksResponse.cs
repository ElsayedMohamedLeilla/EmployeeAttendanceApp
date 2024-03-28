namespace Dawem.Models.Response.Dawem.Requests.Tasks
{
    public class GetRequestTasksResponse
    {
        public List<GetRequestTasksResponseModel> TaskRequests { get; set; }
        public int TotalCount { get; set; }
    }
}
