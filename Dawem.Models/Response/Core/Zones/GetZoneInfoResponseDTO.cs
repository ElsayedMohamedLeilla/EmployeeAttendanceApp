using Dawem.Models.Dtos.Employees.Employees;

namespace Dawem.Models.Response.Core.Zones
{
    public class GetZoneInfoResponseDTO
    {
        public int Code { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public decimal? Radius { get; set; }




    }
}
