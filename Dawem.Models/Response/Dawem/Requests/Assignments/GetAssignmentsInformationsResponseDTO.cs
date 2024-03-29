namespace Dawem.Models.Response.Dawem.Requests.Assignments
{
    public class GetAssignmentsInformationsResponseDTO
    {
        public int TotalAssignmentsCount { get; set; }
        public int AcceptedCount { get; set; }
        public int RejectedCount { get; set; }
        public int PendingCount { get; set; }
    }
}
