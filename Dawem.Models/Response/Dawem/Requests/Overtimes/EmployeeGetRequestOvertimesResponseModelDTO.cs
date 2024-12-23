using Dawem.Enums.Generals;

namespace Dawem.Models.Response.Dawem.Requests.Permissions
{
    public class EmployeeGetRequestOvertimesResponseModelDTO
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public string OvertimeTypeName { get; set; }
        public string StatusName { get; set; }
        public RequestStatus Status { get; set; }
        public DateTime AddedDate { get; set; }
        public DateTime OvertimeDate { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public string DirectManagerName { get; set; }
    }
}
