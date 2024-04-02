using Dawem.Enums.Permissions;

namespace Dawem.Models.Response.Dawem.Permissions.Permissions
{
    public class GetPermissionScreenInfoModel
    {
        public int ScreenCode { get; set; }
        public string ScreenName { get; set; }
        public List<PermissionScreenActionResponseWithNamesModel> PermissionScreenActions { get; set; }
    }
}
