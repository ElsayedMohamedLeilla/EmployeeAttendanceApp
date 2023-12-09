using Dawem.Enums.Generals;

namespace Dawem.Models.Response.Requests.Requests
{
    public class EmployeeGetRequestsResponseModel
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public string StatusName { get; set; }
        public RequestStatus Status { get; set; }
        public DateTime Date { get; set; }
        public RequestType RequestType { get; set; }
        public string RequestTypeName { get; set; }
        public DateTime AddedDate { get; set; }
        public string DirectManagerName { get; set; }
    }
}
