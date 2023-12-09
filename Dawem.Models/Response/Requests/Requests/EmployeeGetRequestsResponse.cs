namespace Dawem.Models.Response.Requests.Requests
{
    public class EmployeeGetRequestsResponse
    {
        public List<EmployeeGetRequestsResponseModel> Requests { get; set; }
        public int TotalCount { get; set; }
    }
}
