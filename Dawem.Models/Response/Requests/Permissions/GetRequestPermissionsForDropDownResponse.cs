namespace Dawem.Models.Response.Requests.Task
{
    public class GetRequestPermissionsForDropDownResponse
    {
        public List<GetRequestPermissionsForDropDownResponseModel> PermissionRequests { get; set; }
        public int TotalCount { get; set; }
    }
}
