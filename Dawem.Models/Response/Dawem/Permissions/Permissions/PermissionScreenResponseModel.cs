using Dawem.Enums.Permissions;

namespace Dawem.Models.Response.Dawem.Permissions.Permissions
{
    public class PermissionScreenResponseModel
    {
        public int ScreenId { get; set; }
        public int ScreenCode { get; set; }
        public List<ApplicationActionCode> ScreenActions { get; set; }
    }
}
