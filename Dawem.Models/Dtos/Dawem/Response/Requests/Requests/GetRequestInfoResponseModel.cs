using Dawem.Enums.Generals;
using Dawem.Models.Dtos.Dawem.Others;

namespace Dawem.Models.Response.Requests.Requests
{
    public class GetRequestInfoResponseModel
    {
        public int Code { get; set; }
        public bool IsNecessary { get; set; }
        public RequestEmployeeModel Employee { get; set; }
        public DateTime Date { get; set; }
        public string StatusName { get; set; }
        public RequestStatus Status { get; set; }
        public List<FileDTO> Attachments { get; set; }
        public bool IsActive { get; set; }
        public RequestType RequestType { get; set; }
        public string RequestTypeName { get; set; }
        public string Notes { get; set; }
    }
}
