namespace Dawem.Models.Response.Core.JustificationsTypes
{
    public class GetJustificationsTypeResponseDTO
    {
        public List<GetJustificationsTypeResponseModelDTO> JustificationsTypes { get; set; }
        public int TotalCount { get; set; }
    }
}
