namespace Dawem.Models.Response.Requests.Vacations
{
    public class GetJustificationsInformationsResponseDTO
    {
        public int TotalJustificationsCount { get; set; }
        public int AcceptedCount { get; set; }
        public int RejectedCount { get; set; }
        public int PendingCount { get; set; }
    }
}
