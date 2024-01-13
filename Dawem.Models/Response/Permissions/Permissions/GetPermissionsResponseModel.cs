using Dawem.Enums.Permissions;

namespace Dawem.Models.Response.Permissions.Permissions
{
    public class GetPermissionsResponseModel
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public ForRoleOrUser ForType { get; set; }
        public string ForTypeName { get; set; }
        public string RoleOrUserName { get; set; }
        public int AllowedScreensCount { get; set; }
        public bool IsActive { get; set; }
    }
}
