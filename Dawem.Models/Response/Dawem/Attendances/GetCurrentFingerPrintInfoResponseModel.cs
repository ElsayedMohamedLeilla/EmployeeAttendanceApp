using Dawem.Enums.Generals;
using Dawem.Models.Response.Dawem.Core.Zones;
using Newtonsoft.Json;

namespace Dawem.Models.Response.Dawem.Attendances
{
    public class GetCurrentFingerPrintInfoResponseModel
    {
        public int? Id { get; set; }
        public int? Code { get; set; }
        public DateTime? CheckInDateTime { get; set; }
        public DateTime? CheckOutDateTime { get; set; }
        public DateTime? BreakInDateTime { get; set; }
        public FingerPrintType? LastFingetPrintType { get; set; }
        [JsonIgnore]
        public FingerPrintType? LastFingetPrintTypeForCheck { get; set; }
        public FingerPrintType DefaultCheckType { get; set; }
        public DateTime LocalDate { get; set; }
        public EmployeeStatus EmployeeStatus { get; set; }
        public List<AvailableZoneDTO> AvailableZones { get; set; }
        public bool AllowFingerprintOutsideAllowedZones { get; set; }
    }
}
