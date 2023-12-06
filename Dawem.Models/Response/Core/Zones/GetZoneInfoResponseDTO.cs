using Dawem.Models.Dtos.Employees.Employees;

namespace Dawem.Models.Response.Core.Zones
{
    public class GetZoneInfoResponseDTO
    {
        public int Code { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double? Radius { get; set; }




    }
}
