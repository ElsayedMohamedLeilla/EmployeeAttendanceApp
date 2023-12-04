namespace Dawem.Models.Response.Requests.Permissions
{
    public class GetRequestPermissionsForDropDownResponse
    {
        public List<GetRequestPermissionsForDropDownResponseModel> PermissionRequests { get; set; }
        public int TotalCount { get; set; }
    }
}
