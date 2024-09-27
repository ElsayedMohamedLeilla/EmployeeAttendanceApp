namespace Dawem.Models.Response.Dawem.Permissions.PermissionLogs
{
    public class GetPermissionLogInfoResponseModel
    {
        public string UserName { get; set; }
        public string ScreenName { get; set; }
        public string ActionName { get; set; }
        public DateTime DateAndTime { get; set; }
    }
}
