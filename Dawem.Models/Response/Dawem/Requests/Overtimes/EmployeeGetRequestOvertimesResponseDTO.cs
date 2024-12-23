namespace Dawem.Models.Response.Dawem.Requests.Permissions
{
    public class EmployeeGetRequestOvertimesResponseDTO
    {
        public List<EmployeeGetRequestOvertimesResponseModelDTO> OvertimeRequests { get; set; }
        public int TotalCount { get; set; }
    }
}
