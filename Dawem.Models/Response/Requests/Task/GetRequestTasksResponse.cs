namespace Dawem.Models.Response.Requests.Task
{
    public class GetRequestTasksResponse
    {
        public List<GetRequestTasksResponseModel> TaskRequests { get; set; }
        public int TotalCount { get; set; }
    }
}
