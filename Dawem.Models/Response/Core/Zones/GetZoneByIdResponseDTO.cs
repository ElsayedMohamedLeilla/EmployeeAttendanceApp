namespace Dawem.Models.Response.Core.Zones
{
    public class GetZoneByIdResponseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public decimal? Radius { get; set; }






    }
}
