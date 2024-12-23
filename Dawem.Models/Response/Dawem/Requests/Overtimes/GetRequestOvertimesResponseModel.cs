using Dawem.Enums.Generals;

namespace Dawem.Models.Response.Dawem.Requests.Permissions
{
    public class GetRequestOvertimesResponseModel
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public RequestEmployeeModel Employee { get; set; }
        public string OvertimeTypeName { get; set; }
        public string StatusName { get; set; }
        public string Period { get; set; }
        public RequestStatus Status { get; set; }
        public DateTime OvertimeDate { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
    }
}
