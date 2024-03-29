namespace Dawem.Models.Response.Dawem.Permissions.Permissions
{
    public class GetPermissionsResponse
    {
        public List<GetPermissionsResponseModel> Permissions { get; set; }
        public int TotalCount { get; set; }
    }
}
