using Dawem.Enums.Generals;

namespace Dawem.Models.Response.Attendances
{
    public class FingerPrintValidationResponseModel
    {
        public int EmployeeId { get; set; }
        public int ScheduleId { get; set; }
        public int ShiftId { get; set; }
        public int? SummonId { get; set; }
        public int? ZoneId { get; set; }
        public DateTime LocalDate { get; set; }
        public TimeOnly ShiftCheckInTime { get; set; }
        public TimeOnly ShiftCheckOutTime { get; set; }
        public int AllowedMinutes { get; set; }
        public FingerPrintType FingerPrintType { get; set; }
    }
}
