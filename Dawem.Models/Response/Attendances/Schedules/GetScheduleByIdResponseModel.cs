using Dawem.Models.Dtos.Attendances.Schedules;

namespace Dawem.Models.Response.Attendances.Schedules
{
    public class GetScheduleByIdResponseModel
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public string Notes { get; set; }
        public List<ScheduleDayUpdateModel> ScheduleDays { get; set; }
    }
}
