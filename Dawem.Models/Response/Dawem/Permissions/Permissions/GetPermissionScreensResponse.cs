namespace Dawem.Models.Response.Dawem.Permissions.Permissions
{
    public class GetPermissionScreensResponse
    {
        public List<GetPermissionScreenInfoModel> PermissionScreens { get; set; }
        public int TotalCount { get; set; }
    }
}
