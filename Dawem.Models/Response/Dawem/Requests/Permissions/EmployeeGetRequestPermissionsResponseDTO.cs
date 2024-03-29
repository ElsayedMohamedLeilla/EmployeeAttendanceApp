namespace Dawem.Models.Response.Dawem.Requests.Permissions
{
    public class EmployeeGetRequestPermissionsResponseDTO
    {
        public List<EmployeeGetRequestPermissionsResponseModelDTO> PermissionRequests { get; set; }
        public int TotalCount { get; set; }
    }
}
