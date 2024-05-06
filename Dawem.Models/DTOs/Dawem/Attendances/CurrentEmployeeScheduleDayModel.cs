namespace Dawem.Models.Dtos.Dawem.Attendances
{
    public class CurrentEmployeeScheduleDayModel
    {
        public int EmployeeId { get; set; }
        public int ScheduleId { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
    }
}
