namespace Dawem.Models.Response.Dawem.Permissions.PermissionLogs
{
    public class GetPermissionLogsResponse
    {
        public List<GetPermissionLogsResponseModel> PermissionLogs { get; set; }
        public int TotalCount { get; set; }
    }
}
