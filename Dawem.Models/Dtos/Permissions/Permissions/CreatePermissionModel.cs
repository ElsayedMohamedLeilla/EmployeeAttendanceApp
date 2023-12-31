namespace Dawem.Models.Dtos.Permissions.Permissions
{
    public class CreatePermissionModel
    {
        public int RoleId { get; set; }
        public List<PermissionScreenModel> PermissionScreens { get; set; }
        public bool IsActive { get; set; }
    }
}
