namespace Dawem.Models.Response.Dawem.Schedules.Schedules
{
    public class GetSchedulesInformationsResponseDTO
    {
        public int TotalCount { get; set; }
        public int ActiveCount { get; set; }
        public int NotActiveCount { get; set; }
        public int DeletedCount { get; set; }
    }
}
