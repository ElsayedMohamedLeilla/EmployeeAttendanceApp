namespace Dawem.Models.Response.Requests.Justifications
{
    public class GetRequestJustificationsForDropDownResponseDTO
    {
        public List<GetRequestJustificationsForDropDownResponseModelDTO> JustificationRequests { get; set; }
        public int TotalCount { get; set; }
    }
}
