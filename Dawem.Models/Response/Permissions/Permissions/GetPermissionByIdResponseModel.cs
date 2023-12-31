namespace Dawem.Models.Response.Permissions.Permissions
{
    public class GetPermissionByIdResponseModel
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public int Code { get; set; }
        public List<PermissionScreenResponseModel> PermissionScreens { get; set; }
        public bool IsActive { get; set; }
    }
}
