namespace Dawem.Models.Response.Dawem.Requests.Tasks
{
    public class GetTasksInformationsResponseDTO
    {
        public int TotalTasksCount { get; set; }
        public int AcceptedCount { get; set; }
        public int RejectedCount { get; set; }
        public int PendingCount { get; set; }
    }
}
