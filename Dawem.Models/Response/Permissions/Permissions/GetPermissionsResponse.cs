namespace Dawem.Models.Response.Permissions.Permissions
{
    public class GetPermissionsResponse
    {
        public List<GetPermissionsResponseModel> Permissions { get; set; }
        public int TotalCount { get; set; }
    }
}
