namespace Dawem.Models.Response.Dawem.Requests.Permissions
{
    public class GetRequestPermissionsForDropDownResponse
    {
        public List<GetRequestPermissionsForDropDownResponseModel> PermissionRequests { get; set; }
        public int TotalCount { get; set; }
    }
}
