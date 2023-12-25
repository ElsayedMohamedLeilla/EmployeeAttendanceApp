namespace Dawem.Models.Response.Requests.Permissions
{
    public class EmployeeGetRequestPermissionsResponseDTO
    {
        public List<EmployeeGetRequestPermissionsResponseModelDTO> PermissionRequests { get; set; }
        public int TotalCount { get; set; }
    }
}
