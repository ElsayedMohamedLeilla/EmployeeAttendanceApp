namespace Dawem.Models.Response.Requests.Task
{
    public class GetRequestPermissionsResponse
    {
        public List<GetRequestPermissionsResponseModel> PermissionRequests { get; set; }
        public int TotalCount { get; set; }
    }
}
