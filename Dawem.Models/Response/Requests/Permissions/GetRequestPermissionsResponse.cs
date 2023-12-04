namespace Dawem.Models.Response.Requests.Permissions
{
    public class GetRequestPermissionsResponse
    {
        public List<GetRequestPermissionsResponseModel> PermissionRequests { get; set; }
        public int TotalCount { get; set; }
    }
}
