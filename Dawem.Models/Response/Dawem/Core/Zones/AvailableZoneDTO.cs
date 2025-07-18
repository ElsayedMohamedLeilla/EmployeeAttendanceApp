﻿namespace Dawem.Models.Response.Dawem.Core.Zones
{
    public class AvailableZoneDTO
    {
        public int ZoneId { get; set; }
        public string Name { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double? Radius { get; set; }
    }
}
