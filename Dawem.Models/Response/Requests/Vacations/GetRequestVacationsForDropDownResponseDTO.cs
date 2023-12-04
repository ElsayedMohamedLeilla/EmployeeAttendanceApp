namespace Dawem.Models.Response.Requests.Vacations
{
    public class GetRequestVacationsForDropDownResponseDTO
    {
        public List<GetRequestVacationsForDropDownResponseModelDTO> VacationRequests { get; set; }
        public int TotalCount { get; set; }
    }
}
