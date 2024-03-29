namespace Dawem.Models.Dtos.Dawem.Schedules.Schedules
{
    public class CreateScheduleModel
    {
        public string Name { get; set; }
        public string Notes { get; set; }
        public bool IsActive { get; set; }
        public List<ScheduleDayCreateModel> ScheduleDays { get; set; }

    }
}
