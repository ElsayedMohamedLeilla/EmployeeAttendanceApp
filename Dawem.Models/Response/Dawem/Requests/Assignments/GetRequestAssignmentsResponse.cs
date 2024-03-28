namespace Dawem.Models.Response.Dawem.Requests.Assignments
{
    public class GetRequestAssignmentsResponse
    {
        public List<GetRequestAssignmentsResponseModel> AssignmentRequests { get; set; }
        public int TotalCount { get; set; }
    }
}
