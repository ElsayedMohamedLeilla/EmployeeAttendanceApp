using Dawem.Enums.Permissions;

namespace Dawem.Models.Response.Dawem.Permissions.Permissions
{
    public class PermissionScreenResponseModel
    {
        public int ScreenCode { get; set; }
        public List<PermissionScreenActionResponseModel> PermissionScreenActions { get; set; }
    }
}
