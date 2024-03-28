namespace Dawem.Models.Response.Dawem.Core.JustificationsTypes
{
    public class GetJustificationsTypeDropDownResponseDTO
    {
        public List<GetJustificationsTypeForDropDownResponseModelDTO> JustificationsTypes { get; set; }
        public int TotalCount { get; set; }
    }
}
