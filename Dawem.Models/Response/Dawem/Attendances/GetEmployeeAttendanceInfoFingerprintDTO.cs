using Dawem.Enums.Generals;

namespace Dawem.Models.Response.Dawem.Attendances
{
    public class GetEmployeeAttendanceInfoFingerprintDTO
    {
        public string ZoneName { get; set; }
        public string Time { get; set; }
        public string Type { get; set; }
        public string RecognitionWay { get; set; }
        public string FingerprintSource { get; set; }
    }
}
