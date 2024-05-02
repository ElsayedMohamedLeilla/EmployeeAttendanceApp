namespace Dawem.Models.Dtos.Dawem.Attendances
{
    public class GetScheduleModel
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public List<GetScheduleDayModel> ScheduleDays { get; set; }
    }
}
