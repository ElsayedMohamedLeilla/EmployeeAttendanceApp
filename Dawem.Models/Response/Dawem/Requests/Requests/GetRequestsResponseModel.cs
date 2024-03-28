using Dawem.Enums.Generals;

namespace Dawem.Models.Response.Dawem.Requests.Requests
{
    public class GetRequestsResponseModel
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public RequestEmployeeModel Employee { get; set; }
        public string StatusName { get; set; }
        public RequestStatus Status { get; set; }
        public DateTime Date { get; set; }
        public RequestType RequestType { get; set; }
        public string RequestTypeName { get; set; }
    }
}
