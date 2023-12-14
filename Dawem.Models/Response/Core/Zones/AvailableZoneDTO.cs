namespace Dawem.Models.Response.Core.Zones
{
    public class AvailableZoneDTO
    {
        public string Name { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double? Radius { get; set; }
    }
}
