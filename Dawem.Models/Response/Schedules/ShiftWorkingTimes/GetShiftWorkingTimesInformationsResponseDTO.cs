namespace Dawem.Models.Response.Requests.Vacations
{
    public class GetShiftWorkingTimesInformationsResponseDTO
    {
        public int TotalCount { get; set; }
        public int ActiveCount { get; set; }
        public int NotActiveCount { get; set; }
        public int DeletedCount { get; set; }
    }
}
