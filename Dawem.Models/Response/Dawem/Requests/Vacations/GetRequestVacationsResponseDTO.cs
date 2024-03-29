namespace Dawem.Models.Response.Dawem.Requests.Vacations
{
    public class GetRequestVacationsResponseDTO
    {
        public List<GetRequestVacationsResponseModelDTO> VacationRequests { get; set; }
        public int TotalCount { get; set; }
    }
}
