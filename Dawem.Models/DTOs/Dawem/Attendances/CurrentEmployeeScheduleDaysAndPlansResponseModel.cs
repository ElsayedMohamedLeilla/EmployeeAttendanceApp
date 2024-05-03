namespace Dawem.Models.Dtos.Dawem.Attendances
{
    public class CurrentEmployeeScheduleDaysAndPlansResponseModel
    {
        public List<CurrentEmployeeScheduleShiftResponseModel> EmployeeScheduleDays { get; set; }
        public List<CurrentEmployeeScheduleDayResponseModel> EmployeeSchedulePlans { get; set; }
        public DateTime Date { get; set; }
    }
}
