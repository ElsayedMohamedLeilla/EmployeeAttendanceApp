namespace Dawem.Models.Response.Attendances.FingerprintDevices
{
    public class GetFingerprintDevicesForDropDownResponse
    {
        public List<BaseGetForDropDownResponseModel> FingerprintDevices { get; set; }
        public int TotalCount { get; set; }
    }
}
