using Dawem.Enums.Permissions;

namespace Dawem.Models.Dtos.Permissions.Permissions
{
    public class UpdatePermissionModel
    {
        public int Id { get; set; }
        public ForRoleOrUser ForType { get; set; }
        public int? RoleId { get; set; }
        public int? UserId { get; set; }
        public List<PermissionScreenModel> PermissionScreens { get; set; }
        public bool IsActive { get; set; }
    }
}
