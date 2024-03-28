using Dawem.Enums.Permissions;

namespace Dawem.Models.Response.Permissions.Permissions
{
    public class PermissionScreenResponseModel
    {
        public ApplicationScreenCode ScreenCode { get; set; }
        public List<PermissionScreenActionResponseModel> PermissionScreenActions { get; set; }
    }
}
