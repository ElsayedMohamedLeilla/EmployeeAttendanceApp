namespace Dawem.Models.Response.Dawem.Requests.Permissions
{
    public class GetRequestPermissionsResponse
    {
        public List<GetRequestPermissionsResponseModel> PermissionRequests { get; set; }
        public int TotalCount { get; set; }
    }
}
