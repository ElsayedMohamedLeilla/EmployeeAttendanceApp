namespace Dawem.Models.Response.Core.Zones
{
    public class GetZoneByIdResponseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double? Radius { get; set; }






    }
}
