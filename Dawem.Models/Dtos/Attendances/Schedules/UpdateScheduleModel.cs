namespace Dawem.Models.Dtos.Attendances.Schedules
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
