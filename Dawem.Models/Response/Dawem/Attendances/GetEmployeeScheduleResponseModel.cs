namespace Dawem.Models.Response.Dawem.Attendances
{
    public class GetEmployeeScheduleResponseModel
    {
        public string DayName { get; set; }
        public bool IsVacation { get; set; }
        public string TimeFrom { get; set; }
        public string TimeTo { get; set; }
        public decimal? WorkingHours { get; set; }
    }
}
