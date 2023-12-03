namespace Dawem.Models.Response.Employees.TaskTypes
{
    public class GetRequestTasksResponse
    {
        public List<GetRequestTasksResponseModel> TaskRequests { get; set; }
        public int TotalCount { get; set; }
    }
}
