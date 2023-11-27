namespace Dawem.Models.Response.Core.Zones
{
    public class GetZoneResponseDTO
    {
        public List<GetZoneResponseModelDTO> Zones { get; set; }
        public int TotalCount { get; set; }
    }
}
