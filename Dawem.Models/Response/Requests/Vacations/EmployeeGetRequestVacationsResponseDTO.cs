namespace Dawem.Models.Response.Requests.Justifications
{
    public class EmployeeGetRequestVacationsResponseDTO
    {
        public List<EmployeeGetRequestVacationsResponseModelDTO> VacationRequests { get; set; }
        public int TotalCount { get; set; }
    }
}
