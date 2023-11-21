namespace Dawem.Models.Response.Schedules.Schedules
{
    public class FingerPrintValidationResponseModel
    {
        public int EmployeeId { get; set; }
        public int ScheduleId { get; set; }
        public int ShiftId { get; set; }
        public DateTime LocalDate { get; set; }
        public TimeOnly ShiftCheckInTime { get; set; }
        public TimeOnly ShiftCheckOutTime { get; set; }
    }
}
