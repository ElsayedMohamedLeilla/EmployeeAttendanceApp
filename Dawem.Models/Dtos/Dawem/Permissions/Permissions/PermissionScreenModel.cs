using Dawem.Enums.Permissions;

namespace Dawem.Models.Dtos.Dawem.Permissions.Permissions
{
    public class PermissionScreenModel
    {
        public ApplicationScreenCode ScreenCode { get; set; }
        public List<PermissionScreenActionModel> PermissionScreenActions { get; set; }
    }
}
