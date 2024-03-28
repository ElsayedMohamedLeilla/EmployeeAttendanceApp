namespace Dawem.Models.Dtos.Dawem.Schedules.Schedules
{
    public class UpdateScheduleModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Notes { get; set; }
        public bool IsActive { get; set; }
        public List<ScheduleDayUpdateModel> ScheduleDays { get; set; }
    }
}
