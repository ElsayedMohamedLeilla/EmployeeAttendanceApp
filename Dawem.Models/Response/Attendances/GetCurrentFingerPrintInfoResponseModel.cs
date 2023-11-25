namespace Dawem.Models.Response.Attendances
{
    public class GetCurrentFingerPrintInfoResponseModel
    {
        public int Code { get; set; }
        public string CheckInTime { get; set; }
        public string CheckOutTime { get; set; }
        public DateTime LocalDate { get; set; }
    }
}
