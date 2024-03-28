using Dawem.Enums.Generals;

namespace Dawem.Models.Response.Dawem.Requests.Assignments
{
    public class GetAssignmentModel
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public string AssignmentTypeName { get; set; }
        public string StatusName { get; set; }
        public RequestStatus Status { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public List<RequestEmployeeModel> Employees { get; set; }
    }
}
