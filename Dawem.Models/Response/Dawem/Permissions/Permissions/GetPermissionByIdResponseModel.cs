using Dawem.Enums.Permissions;

namespace Dawem.Models.Response.Dawem.Permissions.Permissions
{
    public class GetPermissionByIdResponseModel
    {
        public int Id { get; set; }
        public ForRoleOrUser ForType { get; set; }
        public int? RoleId { get; set; }
        public int? UserId { get; set; }
        public int Code { get; set; }
        public List<PermissionScreenResponseModel> PermissionScreens { get; set; }
        public bool IsActive { get; set; }
    }
}
