namespace Dawem.Models.Response.Dawem.Core.Holidays
{
    public class GetHolidaiesInformationsResponseDTO
    {
        public int TotalHolidayCount { get; set; }
        public int UpcomingHolidaiesCount { get; set; }
        public int PastHolidaiesCount { get; set; }
    }
}
