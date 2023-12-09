using Dawem.Enums.Generals;

namespace Dawem.Models.Response.Requests.Justifications
{
    public class EmployeeGetRequestPermissionsResponseModelDTO
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public string PermissionTypeName { get; set; }
        public string StatusName { get; set; }
        public RequestStatus Status { get; set; }
        public DateTime AddedDate { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public string DirectManagerName { get; set; }
    }
}
