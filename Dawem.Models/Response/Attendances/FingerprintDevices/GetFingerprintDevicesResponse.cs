namespace Dawem.Models.Response.Employees.TaskTypes
{
    public class GetFingerprintDevicesResponse
    {
        public List<GetFingerprintDevicesResponseModel> FingerprintDevices { get; set; }
        public int TotalCount { get; set; }
    }
}
