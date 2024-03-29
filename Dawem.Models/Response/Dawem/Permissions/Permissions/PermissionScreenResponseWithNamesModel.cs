using Dawem.Enums.Permissions;

namespace Dawem.Models.Response.Dawem.Permissions.Permissions
{
    public class PermissionScreenResponseWithNamesModel
    {
        public ApplicationScreenCode ScreenCode { get; set; }
        public string ScreenName { get; set; }
        public List<PermissionScreenActionResponseWithNamesModel> PermissionScreenActions { get; set; }
    }
}
