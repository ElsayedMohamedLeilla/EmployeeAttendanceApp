namespace Dawem.Models.Response.Permissions.Permissions
{
    public class GetPermissionScreensResponse
    {
        public List<GetPermissionScreenInfoModel> PermissionScreens { get; set; }
        public int TotalCount { get; set; }
    }
}
