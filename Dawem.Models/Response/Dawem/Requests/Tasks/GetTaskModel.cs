using Dawem.Enums.Generals;

namespace Dawem.Models.Response.Dawem.Requests.Tasks
{
    public class GetTaskModel
    {
        public int Id { get; set; }
        public int RequestId { get; set; }
        public int Code { get; set; }
        public string TaskTypeName { get; set; }
        public string StatusName { get; set; }
        public RequestStatus Status { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public List<RequestEmployeeModel> Employees { get; set; }
    }
}
