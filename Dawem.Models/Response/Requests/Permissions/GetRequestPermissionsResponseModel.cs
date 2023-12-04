using Dawem.Enums.Generals;

namespace Dawem.Models.Response.Requests.Permissions
{
    public class GetRequestPermissionsResponseModel
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public RequestEmployeeModel Employee { get; set; }
        public string PermissionTypeName { get; set; }
        public string StatusName { get; set; }
        public RequestStatus Status { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
    }
}
