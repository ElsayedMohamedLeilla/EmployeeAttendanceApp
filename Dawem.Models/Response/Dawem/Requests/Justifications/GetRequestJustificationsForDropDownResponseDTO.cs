namespace Dawem.Models.Response.Dawem.Requests.Justifications
{
    public class GetRequestJustificationsForDropDownResponseDTO
    {
        public List<GetRequestJustificationsForDropDownResponseModelDTO> JustificationRequests { get; set; }
        public int TotalCount { get; set; }
    }
}
