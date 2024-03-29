namespace Dawem.Models.Response.Dawem.Employees.TaskTypes
{
    public class GetTaskTypesForDropDownResponse
    {
        public List<GetTaskTypesForDropDownResponseModel> TaskTypes { get; set; }
        public int TotalCount { get; set; }
    }
}
