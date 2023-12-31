namespace Dawem.Models.Response.Permissions.Permissions
{
    public class GetPermissionsResponseModel
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public string RoleName { get; set; }
        public int AllowedScreensCount { get; set; }
        public bool IsActive { get; set; }
    }
}
