namespace Dawem.Models.Response.Schedules.Schedules
{
    public class GetCurrentAttendanceInfoResponseModel
    {
        public int Code { get; set; }
        public TimeOnly CheckInTime { get; set; }
        public TimeOnly? CheckOutTime { get; set; }
        public DateTime LocalDate { get; set; }
    }
}
