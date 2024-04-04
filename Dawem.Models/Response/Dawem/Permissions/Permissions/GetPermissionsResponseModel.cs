using Dawem.Enums.Permissions;

namespace Dawem.Models.Response.Dawem.Permissions.Permissions
{
    public class GetPermissionsResponseModel
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public ForResponsibilityOrUser ForType { get; set; }
        public string ForTypeName { get; set; }
        public string ResponsibilityOrUserName { get; set; }
        public int AllowedScreensCount { get; set; }
        public bool IsActive { get; set; }
    }
}
