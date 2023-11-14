using Dawem.Models.Dtos.Attendances.Schedules;

namespace Dawem.Models.Response.Attendances.Schedules
{
    public class GetScheduleInfoResponseModel
    {
        public int Code { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public string Notes { get; set; }
        public List<ScheduleShiftTextModel> ScheduleDays { get; set; }
    }
}
