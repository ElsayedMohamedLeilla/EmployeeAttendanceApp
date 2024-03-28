namespace Dawem.Models.Response.Core.Zones
{
    public class GetZoneDropDownResponseDTO
    {
        public List<GetZoneForDropDownResponseModelDTO> Zones { get; set; }
        public int TotalCount { get; set; }
    }
}
