namespace Dawem.Models.Dtos.Dawem.Attendances
{
    public class CurrentEmployeeScheduleDayResponseModel
    {
        public DateTime DateFrom { get; set; }
        public List<CurrentEmployeeScheduleShiftResponseModel> ScheduleDays { get; set; }
    }
}
