namespace Dawem.Models.Response.Dawem.Employees.Employees
{
    public class GetCurrentEmployeeScheduleInPeriodDTO
    {
        public string Name { get; set; }
        public string DayName { get; set; }
        public DateTime Date { get; set; }
        public string ScheduleName { get; set; }
        public TimeSpan CheckInTime { get; set; }
        public TimeSpan CheckOutTime { get; set; }

    }
}
