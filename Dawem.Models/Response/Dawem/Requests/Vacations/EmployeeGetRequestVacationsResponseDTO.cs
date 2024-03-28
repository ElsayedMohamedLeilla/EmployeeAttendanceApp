namespace Dawem.Models.Response.Dawem.Requests.Vacations
{
    public class EmployeeGetRequestVacationsResponseDTO
    {
        public List<EmployeeGetRequestVacationsResponseModelDTO> VacationRequests { get; set; }
        public int TotalCount { get; set; }
    }
}
