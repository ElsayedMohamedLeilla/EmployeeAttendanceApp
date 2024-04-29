using Dawem.Enums.Generals;
using Dawem.Models.Response.Dawem.Core.Zones;

namespace Dawem.Models.Response.Dawem.Attendances
{
    public class GetCurrentFingerPrintInfoResponseModel
    {
        public int? Id { get; set; }
        public int? Code { get; set; }
        public DateTime CheckInDateTime { get; set; }
        public DateTime CheckOutDateTime { get; set; }
        public FingerprintCheckType DefaultCheckType { get; set; }
        public DateTime LocalDate { get; set; }
        public EmployeeStatus EmployeeStatus { get; set; }
        public List<AvailableZoneDTO> AvailableZones { get; set; }
    }
}
