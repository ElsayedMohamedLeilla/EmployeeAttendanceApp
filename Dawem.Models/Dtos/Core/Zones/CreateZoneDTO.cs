namespace Dawem.Models.Dtos.Core.Zones
{
    public class CreateZoneDTO
    {
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public decimal? Radius { get; set; }




    }
}
