namespace Dawem.Models.Response.Employees.AssignmentTypes
{
    public class GetPermissionLogsResponse
    {
        public List<GetPermissionLogsResponseModel> PermissionLogs { get; set; }
        public int TotalCount { get; set; }
    }
}
