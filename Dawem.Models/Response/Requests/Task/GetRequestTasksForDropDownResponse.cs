namespace Dawem.Models.Response.Employees.TaskTypes
{
    public class GetRequestTasksForDropDownResponse
    {
        public List<GetRequestTasksForDropDownResponseModel> TaskRequests  { get; set; }
        public int TotalCount { get; set; }
    }
}
