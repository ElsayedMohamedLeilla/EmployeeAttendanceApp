namespace Dawem.Models.Response.Employees.TaskTypes
{
    public class GetFingerprintDevicesForDropDownResponse
    {
        public List<GetFingerprintDeviceForDropDownResponseModel> FingerprintDevices { get; set; }
        public int TotalCount { get; set; }
    }
}
