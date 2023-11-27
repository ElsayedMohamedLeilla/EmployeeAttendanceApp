namespace Dawem.Models.Response.Core.Zones
{
    public class GetZoneResponseModelDTO
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public decimal? Radius { get; set; }



    }
}
