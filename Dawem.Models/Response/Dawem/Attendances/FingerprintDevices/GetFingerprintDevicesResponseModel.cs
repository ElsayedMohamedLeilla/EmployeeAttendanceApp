namespace Dawem.Models.Response.Dawem.Attendances.FingerprintDevices
{
    public class GetFingerprintDevicesResponseModel
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public string Name { get; set; }
        public DateTime LastSeenDate { get; set; }
        public bool IsActive { get; set; }
    }
}
