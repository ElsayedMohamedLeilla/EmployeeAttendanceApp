namespace Dawem.Models.Response.Dawem.Dashboard
{
    public class GetRequestsStatusResponseModel
    {
        public int AcceptedCount { get; set; }
        public int RejectedCount { get; set; }
        public int PendingCount { get; set; }
    }
}
