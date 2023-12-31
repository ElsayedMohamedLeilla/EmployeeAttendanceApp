namespace Dawem.Models.Response.Permissions.Permissions
{
    public class GetPermissionInfoResponseModel
    {
        public int Id { get; set; }
        public string RoleName { get; set; }
        public int Code { get; set; }
        public List<PermissionScreenResponseWithNamesModel> PermissionScreens { get; set; }
        public bool IsActive { get; set; }
    }
}
