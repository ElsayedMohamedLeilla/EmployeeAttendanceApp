namespace Dawem.Models.Dtos.Dawem.Attendances
{
    public class CheckIfShift24HoursModel
    {
        public DateTime ClientLocalDateTime { get; set; }
        public int EmployeeId { get; set; }
        public int ScheduleId { get; set; }
    }
}
