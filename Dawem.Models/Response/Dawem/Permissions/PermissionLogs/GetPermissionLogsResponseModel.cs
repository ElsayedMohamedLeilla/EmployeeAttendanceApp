namespace Dawem.Models.Response.Dawem.Permissions.PermissionLogs
{
    public class GetPermissionLogsResponseModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string ScreenName { get; set; }
        public bool IsActive { get; set; }
    }
}
