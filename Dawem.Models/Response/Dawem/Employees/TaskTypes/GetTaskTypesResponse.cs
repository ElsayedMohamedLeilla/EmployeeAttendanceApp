namespace Dawem.Models.Response.Dawem.Employees.TaskTypes
{
    public class GetTaskTypesResponse
    {
        public List<GetTaskTypesResponseModel> TaskTypes { get; set; }
        public int TotalCount { get; set; }
    }
}
