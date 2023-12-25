namespace Dawem.Models.Response.Core.JustificationsTypes
{
    public class GetJustificationsTypeDropDownResponseDTO
    {
        public List<GetJustificationsTypeForDropDownResponseModelDTO> JustificationsTypes { get; set; }
        public int TotalCount { get; set; }
    }
}
