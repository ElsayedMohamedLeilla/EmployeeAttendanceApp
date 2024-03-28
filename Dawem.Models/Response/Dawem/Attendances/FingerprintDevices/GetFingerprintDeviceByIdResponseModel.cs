namespace Dawem.Models.Response.Dawem.Attendances.FingerprintDevices
{
    public class GetFingerprintDeviceByIdResponseModel
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public string Name { get; set; }
        public string IpAddress { get; set; }
        public string PortNumber { get; set; }
        public string Model { get; set; }
        public string SerialNumber { get; set; }
        public bool IsActive { get; set; }
    }
}
