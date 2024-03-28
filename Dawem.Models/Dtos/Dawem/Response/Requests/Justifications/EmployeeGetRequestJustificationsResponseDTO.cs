namespace Dawem.Models.Response.Requests.Justifications
{
    public class EmployeeGetRequestJustificationsResponseDTO
    {
        public List<EmployeeGetRequestJustificationsResponseModelDTO> JustificationRequests { get; set; }
        public int TotalCount { get; set; }
    }
}
