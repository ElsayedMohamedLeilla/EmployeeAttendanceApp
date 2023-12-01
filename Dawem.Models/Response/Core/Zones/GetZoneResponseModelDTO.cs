namespace Dawem.Models.Response.Core.Zones
{
    public class GetZoneResponseModelDTO
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public decimal? Radius { get; set; }



    }
}
