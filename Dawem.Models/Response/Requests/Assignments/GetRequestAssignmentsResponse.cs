namespace Dawem.Models.Response.Requests.Assignments
{
    public class GetRequestAssignmentsResponse
    {
        public List<GetRequestAssignmentsResponseModel> AssignmentRequests { get; set; }
        public int TotalCount { get; set; }
    }
}
