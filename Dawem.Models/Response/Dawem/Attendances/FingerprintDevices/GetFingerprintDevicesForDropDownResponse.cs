namespace Dawem.Models.Response.Dawem.Attendances.FingerprintDevices
{
    public class GetFingerprintDevicesForDropDownResponse
    {
        public List<BaseGetForDropDownResponseModel> FingerprintDevices { get; set; }
        public int TotalCount { get; set; }
    }
}
