namespace Dawem.Models.Response.Dawem.Permissions.Permissions
{
    public class GetPermissionScreenInfoModel
    {
        public string ScreenName { get; set; }
        public List<PermissionScreenActionResponseWithNamesModel> PermissionScreenActions { get; set; }
    }
}
