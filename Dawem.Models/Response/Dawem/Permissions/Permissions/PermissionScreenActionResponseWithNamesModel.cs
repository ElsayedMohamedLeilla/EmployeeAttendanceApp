using Dawem.Enums.Permissions;

namespace Dawem.Models.Response.Dawem.Permissions.Permissions
{
    public class PermissionScreenActionResponseWithNamesModel
    {
        public DawemAdminApplicationAction ActionCode { get; set; }
        public string ActionName { get; set; }
    }
}
