namespace Dawem.Models.Response.Dawem.Requests.Requests
{
    public class EmployeeGetRequestsResponse
    {
        public List<EmployeeGetRequestsResponseModel> Requests { get; set; }
        public int TotalCount { get; set; }
    }
}
