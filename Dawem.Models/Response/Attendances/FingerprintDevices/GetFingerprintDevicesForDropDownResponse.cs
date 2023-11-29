namespace Dawem.Models.Response.Employees.TaskTypes
{
    public class GetFingerprintDevicesForDropDownResponse
    {
        public List<BaseGetForDropDownResponseModel> FingerprintDevices { get; set; }
        public int TotalCount { get; set; }
    }
}
