﻿namespace Dawem.Models.Dtos.Dawem.Core.Zones
{
    public class CreateZoneDTO
    {
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public decimal? Radius { get; set; }

    }
}
