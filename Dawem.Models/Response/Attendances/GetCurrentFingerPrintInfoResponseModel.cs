namespace Dawem.Models.Response.Schedules.Schedules
{
    public class GetCurrentFingerPrintInfoResponseModel
    {
        public int Code { get; set; }
        public string CheckInTime { get; set; }
        public string CheckOutTime { get; set; }
        public DateTime LocalDate { get; set; }
    }
}
