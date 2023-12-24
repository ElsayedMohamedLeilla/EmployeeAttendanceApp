namespace Dawem.Models.Response.Employees.Employee
{
    public class GetRequestsStatusResponseModel
    {
        public int AcceptedCount { get; set; }
        public int RejectedCount { get; set; }
        public int PendingCount { get; set; }
    }
}
