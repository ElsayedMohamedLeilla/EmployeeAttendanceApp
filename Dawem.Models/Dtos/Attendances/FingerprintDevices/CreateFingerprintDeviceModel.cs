namespace Dawem.Models.Dtos.Attendances.FingerprintDevices
{
    public class CreateFingerprintDeviceModel
    {
        public string Name { get; set; }
        public string IpAddress { get; set; }
        public string PortNumber { get; set; }
        public string Model { get; set; }
        public string SerialNumber { get; set; }
        public bool IsActive { get; set; }
    }
}
