namespace Dawem.Models.Response.Requests.Task
{
    public class GetRequestTasksForDropDownResponse
    {
        public List<GetRequestTasksForDropDownResponseModel> TaskRequests { get; set; }
        public int TotalCount { get; set; }
    }
}
