namespace Dawem.Models.Response.Dawem.Attendances.FingerprintDevices
{
    public class GetFingerprintDevicesResponse
    {
        public List<GetFingerprintDevicesResponseModel> FingerprintDevices { get; set; }
        public int TotalCount { get; set; }
    }
}
