namespace Dawem.Models.Response.Attendances.FingerprintDevices
{
    public class GetFingerprintDevicesResponse
    {
        public List<GetFingerprintDevicesResponseModel> FingerprintDevices { get; set; }
        public int TotalCount { get; set; }
    }
}
