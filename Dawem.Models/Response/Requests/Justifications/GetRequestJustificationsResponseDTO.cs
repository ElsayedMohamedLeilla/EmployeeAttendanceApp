namespace Dawem.Models.Response.Requests.Justifications
{
    public class GetRequestJustificationsResponseDTO
    {
        public List<GetRequestJustificationsResponseModelDTO> JustificationRequests { get; set; }
        public int TotalCount { get; set; }
    }
}
