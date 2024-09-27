using Dawem.Enums.Generals;

namespace Dawem.Models.Response.Dawem.Attendances
{
    public class FingerPrintValidationResponseModel
    {
        public int EmployeeId { get; set; }
        public int ScheduleId { get; set; }
        public int ShiftId { get; set; }
        public int? SummonId { get; set; }
        public int? ZoneId { get; set; }
        public DateTime LocalDateTime { get; set; }
        public TimeSpan ShiftCheckInTime { get; set; }
        public TimeSpan ShiftCheckOutTime { get; set; }
        public bool IsTwoDaysShift { get; set; }
        public int AllowedMinutes { get; set; }
        public FingerPrintType FingerPrintType { get; set; }
    }
}
